using Domain.Interfaces;
using Elasticsearch.Net;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ElasticSearch
{
    public class ESServer : IESServer
    {
        public IElasticClient ElasticLinqClient { get; set; }
        public IElasticLowLevelClient ElasticJsonClient { get; set; }
        public ESServer(IOptionsMonitor<ElasticSearchConfiguration> options)
        {
            var connectionPool = new StaticConnectionPool(options.CurrentValue.Urls.Select(f => new Uri(f)));        //配置请求池
            var settings = new ConnectionSettings(connectionPool)
                               //.BasicAuthentication("cwy","123456")       //配置用户名和密码
                               .DefaultMappingFor<ESDocument>(f=>f.IndexName("cwy516project"))
                               .RequestTimeout(TimeSpan.FromSeconds(30));   //请求配置参数
            ElasticLinqClient = new ElasticClient(settings);
            ElasticJsonClient = new ElasticLowLevelClient(settings);
        }

        
    }
}
