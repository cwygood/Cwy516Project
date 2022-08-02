using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Polly
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _facotry;
        public HttpMessageHandler MessageHandler { get; set; }
        private readonly IOptionsMonitor<HttpClientConfiguration> _options;
        private readonly string _host;

        public HttpService(IHttpClientFactory factory, IOptionsMonitor<HttpClientConfiguration> options, IConfiguration configuration)
        {
            this._facotry = factory;
            this._options = options;
            _host = configuration.GetValue<string>("urls");
        }

        private HttpClient GetClient()
        {
            HttpClient httpClient = null;
            if (this.MessageHandler != null)
            {
                httpClient = new HttpClient(this.MessageHandler);
            }
            else
            {
                httpClient = this._facotry.CreateClient(this._options.CurrentValue.PollyName);
            }
            return httpClient;
        }

        public async Task<T> GetAsync<T>(string url) where T : class
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var res = await this.GetClient().SendAsync(requestMessage);
            string httpJsonString = res.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(httpJsonString);
        }

        public async Task<T> PostAsync<T>(string url, Dictionary<string,object> paras) where T : class
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(paras), Encoding.UTF8, "application/json");
            var res = await this.GetClient().SendAsync(requestMessage);
            string httpJsonString = res.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(httpJsonString);
        }
        /// <summary>
        /// 按照urlencode传递参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public async Task<T> PostUrlEncodeAsync<T>(string url, Dictionary<string, string> paras) where T : class
        {
            if (url.StartsWith("/"))
            {
                url = _host + url;
            }
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            List<KeyValuePair<string, string>> nameVals = new List<KeyValuePair<string, string>>();
            //使用FormUrlEncodedContent的方式传递参数
            if (paras != null)
            {
                foreach(var pair in paras)
                {
                    nameVals.Add(pair);
                }
            }
            request.Content = new FormUrlEncodedContent(nameVals);
            var res = await this.GetClient().SendAsync(request);
            string httpJsonString = res.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(httpJsonString);
        }

        public async Task<string> GetAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var res = await this.GetClient().SendAsync(requestMessage);
            string httpJsonString = res.Content.ReadAsStringAsync().Result;

            return httpJsonString;
        }
    }
}
