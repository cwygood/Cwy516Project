using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConsulConfiguration
    {
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string HealthCheck { get; set; }
        public string ConsulAddress { get; set; }
    }
}
