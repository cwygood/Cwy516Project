using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.ElasticSearch
{
    public class ESDocument
    {
        public string Id { get; set; }
        public object data { get; set; }
    }
}
