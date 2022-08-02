using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class MongoDbConfiguration
    {
        /// <summary>
        /// 连接串
        /// </summary>
        public string Connectionstring { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
