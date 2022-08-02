using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }
        public string ReadConnectionString { get; set; }
    }
}
