using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IESServer
    {
        IElasticClient ElasticLinqClient { get; set; }

        IElasticLowLevelClient ElasticJsonClient { get; set; }
    }
}
