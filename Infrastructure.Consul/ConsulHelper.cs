using Consul;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Consul
{
    public class ConsulHelper : IConsulHelper
    {
        private readonly ConsulClient _consulClient;
        private readonly IOptionsMonitor<ConsulConfiguration> _options;
        private readonly ConcurrentDictionary<string, string[]> _serviceUrls = new ConcurrentDictionary<string, string[]>();
        public ConsulHelper(IOptionsMonitor<ConsulConfiguration> options)
        {
            this._options = options;
            this._consulClient = new ConsulClient(c =>
              {
                  c.Address = new Uri(options.CurrentValue.ConsulAddress);
              });
        }
        public async Task<string> GetServiceUrl(string serviceName)
        {
            this._serviceUrls.TryGetValue(serviceName, out string[] urls);
            if (!urls.Any())
            {
                return "";
            }
            var rmd = new Random().Next(0, urls.Count());//随机获取服务地址
            return await Task.FromResult(urls[rmd]);
        }

        public void GetService()
        {
            var serviceNames = new List<string>() { "TestServiceB", "TestServiceA" };//调用的时候，按照服务名称去依次获取对应的地址
            serviceNames.ForEach(f =>
            {
                Task.Run(() =>
                {
                    var queryOption = new QueryOptions() { WaitTime = TimeSpan.FromSeconds(10) };//每10秒检查一次，如果服务端和客户端版本号不一致，就更新
                    while (true)
                    {
                        GetService(queryOption, f);
                    }
                });
            });
        }

        private void GetService(QueryOptions queryOptions, string serviceName)
        {
            var res = this._consulClient.Health.Service(serviceName, null, true, queryOptions).Result;
            //Console.WriteLine($"{DateTime.Now}获取{serviceName},WaitIndex:{queryOptions.WaitIndex},LastIndex:{res.LastIndex}");
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                //版本号不一致，说明服务器列表发生了变化
                queryOptions.WaitIndex = res.LastIndex;
                var urls = res.Response.Select(f => $"http://{f.Service.Address}:{f.Service.Port}").ToArray();
                _serviceUrls.AddOrUpdate(serviceName, urls, (f,v) => v);
            }
        }
    }
}
