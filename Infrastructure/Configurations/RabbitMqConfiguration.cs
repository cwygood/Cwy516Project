using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class RabbitMqConfiguration
    {
        public RabbitMqHostInfo HostInfo { get; set; }
        public RabbitMqClusterInfo Cluster { get; set; }
    }
    public class RabbitMqClusterInfo
    {
        public RabbitMqHostInfo Producter { get; set; }
        public IEnumerable<RabbitMqHostInfo> Consumer { get; set; }
    }
    public class RabbitMqHostInfo
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
