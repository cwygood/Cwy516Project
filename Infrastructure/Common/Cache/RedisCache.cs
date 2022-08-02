using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Common.Cache
{
    public class RedisCache : IRedisCache
    {
        private readonly int _defaultDb;
        private readonly ConnectionMultiplexer _multiplexer;
        private readonly string _connectString;

        private readonly Domain.Enums.RedisType _redisType;

        private readonly ConnectionMultiplexer _readMultiplexer;//读库操作对象
        private readonly ConnectionMultiplexer _writeMultiplexer;//写库操作对象
        private readonly string _readConnectString;
        private readonly string _writeConnectString;
        private readonly int _readDefaultDb;
        private readonly int _writeDefaultDb;
        public RedisCache(IOptionsMonitor<RedisConfiguration> options)
        {
            this._redisType = options.CurrentValue.RedisType;
            if (this._redisType == Domain.Enums.RedisType.Cluster && options.CurrentValue.ClusterPoints?.Count() > 0)
            {
                //Cluster模式
                var clusterPoints = options.CurrentValue.ClusterPoints;

                #region 单个链接，依赖当前链接必须联通

                //var ss = clusterPoints.FirstOrDefault();
                //this._connectString = $"{ss.Host}:{ss.Port},defaultDatabase={ss.DBIndex},password={ss.Password}";
                //this._multiplexer = ConnectionMultiplexer.Connect(this._connectString);
                #endregion

                ConfigurationOptions clusterOptions = new ConfigurationOptions();
                foreach (var clusterPoint in clusterPoints)
                {
                    clusterOptions.EndPoints.Add(clusterPoint.Host, clusterPoint.Port);
                }
                clusterOptions.AllowAdmin = true;
                clusterOptions.Password = clusterPoints.FirstOrDefault().Password;
                this._multiplexer = ConnectionMultiplexer.Connect(clusterOptions);
            }
            else if (this._redisType == Domain.Enums.RedisType.Sentinel && options.CurrentValue.SentinelPoints?.Count() > 0)
            {
                //哨兵模式
                ConfigurationOptions sentinelOptions = new ConfigurationOptions();
                foreach (var sentinelPoint in options.CurrentValue.SentinelPoints)
                {
                    sentinelOptions.EndPoints.Add(sentinelPoint.Host, sentinelPoint.Port);
                }
                sentinelOptions.DefaultDatabase = options.CurrentValue.HostConfig.DBIndex;
                sentinelOptions.TieBreaker = "";//多主机服务器部署情形下中用于选择出master服务的Key
                sentinelOptions.CommandMap = CommandMap.Sentinel;
                sentinelOptions.AbortOnConnectFail = false;
                sentinelOptions.AllowAdmin = true;//启用被认定为是有风险的一些命令

                ConnectionMultiplexer sentinelConnection = ConnectionMultiplexer.SentinelConnect(sentinelOptions);

                ConfigurationOptions serviceOptions = new ConfigurationOptions();
                serviceOptions.ServiceName = options.CurrentValue.SentinelPoints.FirstOrDefault().MasterName;
                serviceOptions.Password = options.CurrentValue.HostConfig.Password;
                serviceOptions.AbortOnConnectFail = true;
                serviceOptions.AllowAdmin = true;
                this._multiplexer = sentinelConnection.GetSentinelMasterConnection(serviceOptions);
                /*
                 * 正常情况下StackExchange.Redis会自动判别主从节点。但是,如果你没有使用类似于redis-sentinel或者redis cluster的管理工具，你可能会碰到有多个master节点的情况（例如，维护时重置了节点，在网络中在就表现为是一个master）。
                 * 为了解决这种情况，StackExchange.Redis使用了tie-breaker的概念 - 他只有在检测到多个master的时候才会用到。（不包括redis cluster的情况下，因为redis cluster是正是需要多个master的）。为了兼容BookSleeve，默认的key
                 * 是"__Booksleeve_TieBreak"（只存在于0号数据库）。他是一种原始的投票机制去判断更适用的master。(so that work is routed correctly.)类似的，当配置发生改变时（特别是master/slave配置），让已经链接的实例知道新的环境（通过INFO, 
                 * CONFIG等可以使用的命令）是很重要的。StackExchagnge.Redist通过自动订阅一个pub/sub频道，这个频道是用来发送这些通知的。为了兼容BookSleeve，频道名名称默认是 "__Booksleeve_MasterChanged"。这两个选项都是可以通过.ConfigurationChannel
                 * 和.TieBreaker配置属性自定义或者禁用的（设置为""）。这些设置也可以被IServer.MakeMaster()方法使用，来设置数据库里的tie-breaker以及广播配置更改消息。对于master/slave变化的配置信息也可以通过 ConnectionMultiplexer.PublishReconfigure 
                 * 方法来请求所有节点刷新配置。
                 */
            }
            else if (this._redisType == Domain.Enums.RedisType.MasterSlave && options.CurrentValue.MasterSlaves != null)
            {
                var master = options.CurrentValue.MasterSlaves.Master;
                var slave = options.CurrentValue.MasterSlaves.Slaves.FirstOrDefault();//暂时默认取第一个
                //主库（写库）
                this._writeConnectString = $"{master.Host}:{master.Port},defaultDatabase={master.DBIndex},password={master.Password}";
                this._writeMultiplexer = ConnectionMultiplexer.Connect(this._writeConnectString);
                this._writeDefaultDb = master.DBIndex;

                //从库（读库）
                this._readConnectString = $"{slave.Host}:{slave.Port},defaultDatabase={slave.DBIndex},password={slave.Password}";
                this._readMultiplexer = ConnectionMultiplexer.Connect(this._readConnectString);
                this._readDefaultDb = slave.DBIndex;
            }
            else
            {
                var hostConfig = options.CurrentValue.HostConfig;
                this._defaultDb = hostConfig.DBIndex;
                this._connectString = $"{hostConfig.Host}:{hostConfig.Port},defaultDatabase={hostConfig.DBIndex},password={hostConfig.Password}";
                this._multiplexer = ConnectionMultiplexer.Connect(this._connectString);
            }
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase(int type = 0)
        {
            if (this._redisType == Domain.Enums.RedisType.MasterSlave)
            {
                switch (type)
                {
                    case 1:
                        return this._readMultiplexer.GetDatabase(this._readDefaultDb);
                    case 2:
                        return this._writeMultiplexer.GetDatabase(this._writeDefaultDb);
                }
            }
            return this._multiplexer.GetDatabase(this._defaultDb);
        }
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="type">0：正常redis服务器，1：读库服务器，2：写库服务器</param>
        /// <returns></returns>
        public IServer GetServer(int type = 0)
        {
            if (this._redisType == Domain.Enums.RedisType.MasterSlave)
            {
                switch (type)
                {
                    case 1:
                        return this._readMultiplexer.GetServer(this._readConnectString);
                    case 2:
                        return this._writeMultiplexer.GetServer(this._writeConnectString);
                }
            }
            return this._multiplexer.GetServer(this._connectString);
        }

        public ISubscriber GetSubscriber()
        {
            return this._multiplexer.GetSubscriber();
        }
        public bool SetString(string key, string value, TimeSpan? span = null, string prefix = "")
        {
            var curKey = key;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                curKey = prefix + ":" + key;
            }
            return this.GetDatabase(2).StringSet(curKey, value, span);
        }
        public string GetString(string key, string prefix = "")
        {
            var curKey = key;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                curKey = prefix + ":" + key;
            }
            return this.GetDatabase(1).StringGet(curKey);
        }

        public void Dispose()
        {
            
        }
    }
}
