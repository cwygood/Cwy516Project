using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum RedisType
    {
        /// <summary>
        /// 单机模式
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 主从模式
        /// </summary>
        MasterSlave = 1,
        /// <summary>
        /// 哨兵模式
        /// </summary>
        Sentinel = 2,
        /// <summary>
        /// 分片模式
        /// </summary>
        Cluster = 3
    }
}
