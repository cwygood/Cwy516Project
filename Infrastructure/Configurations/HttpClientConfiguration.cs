using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// HttpClient熔断降级策略配置
    /// </summary>
    public class HttpClientConfiguration
    {
        public HttpClientConfiguration()
        {
            this.PollyName = "Polly";
            this.TimeoutTime = 60;
            this.RetryCount = 3;
            this.CircuitBreakerOpenFallCount = 2;
            this.CircuitBreakerDownTime = 3;
            this.ResponseMessage = "服务熔断降级";
        }
        public string PollyName { get; set; }
        /// <summary>
        /// 超时时间设置：单位（秒）
        /// </summary>
        public int TimeoutTime { get; set; }
        /// <summary>
        /// 失败重试次数
        /// </summary>
        public int RetryCount { get; set; }
        /// <summary>
        /// 失败多少次，开启断路器（例如：失败2次，开启断路器）
        /// </summary>
        public int CircuitBreakerOpenFallCount { get; set; }
        /// <summary>
        /// 断路器开启时间（例如：断路器开启2秒后，自动关闭）
        /// </summary>
        public int CircuitBreakerDownTime { get; set; }
        /// <summary>
        /// 降级处理(将异常消息封装成为正常消息返回，然后进行响应处理，例如：系统正在繁忙，请稍后处理.....)
        /// </summary>
        public string ResponseMessage { get; set; }
    }
}
