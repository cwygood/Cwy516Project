using Domain.Interfaces;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ConsulRepository : IConsulRepository
    {
        private readonly IConsulHelper _consulHelper;
        private readonly IHttpService _httpService;
        public ConsulRepository(IConsulHelper consulHelper, IHttpService httpService)
        {
            this._consulHelper = consulHelper;
            this._httpService = httpService;
        }
        public async Task<string> SendRequest(string serviceName)
        {
            var serviceUrl = await this._consulHelper.GetServiceUrl(serviceName);
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                return await Task.FromResult($"{serviceName}服务可用地址为空");
            }
            return await this._httpService.GetAsync(serviceUrl + "/api/home/get");
        }
    }
}
