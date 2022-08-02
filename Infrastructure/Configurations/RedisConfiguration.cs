using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class RedisConfiguration
    {
        public RedisType RedisType { get; set; }
        /// <summary>
        /// 单机模式
        /// </summary>
        public RedisHostConfig HostConfig { get; set; }
        /// <summary>
        /// 主从模式
        /// </summary>
        public MasterSlave MasterSlaves { get; set; }
        /// <summary>
        /// 哨兵模式（Sentinel）
        /// </summary>
        public IEnumerable<RedisHostConfig> SentinelPoints { get; set; }
        /// <summary>
        /// 分片模式（Cluster）
        /// </summary>
        public IEnumerable<RedisHostConfig> ClusterPoints { get; set; }
    }

    public class MasterSlave
    {
        /// <summary>
        /// 主库（写库）
        /// </summary>
        public RedisHostConfig Master { get; set; }
        /// <summary>
        /// 从库（读库）
        /// </summary>
        public IEnumerable<RedisHostConfig> Slaves { get; set; }
    }

    public class RedisHostConfig
    {
        public string MasterName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int DBIndex { get; set; }
    }
}
