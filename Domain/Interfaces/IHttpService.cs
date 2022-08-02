using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IHttpService
    {
        HttpMessageHandler MessageHandler { get; set; }
        Task<T> GetAsync<T>(string url) where T : class;
        Task<string> GetAsync(string url);
        Task<T> PostAsync<T>(string url, Dictionary<string, object> paras) where T : class;
        Task<T> PostUrlEncodeAsync<T>(string url, Dictionary<string, string> paras) where T : class;
    }
}
