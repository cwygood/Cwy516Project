using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Polly;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddPollyHttpClient(this IServiceCollection services, IConfigurationSection section, Func<HttpMessageHandler> func = null)
    {
        //读取配置文件信息
        var cfg = section.Get<HttpClientConfiguration>();
        //封装降级信息
        var fallbackResponse = new HttpResponseMessage()
        {
            Content = new StringContent(cfg.ResponseMessage),
            StatusCode = System.Net.HttpStatusCode.GatewayTimeout
        };
        //配置httpclient熔断降级策略
        services.AddHttpClient(cfg.PollyName)
            .ConfigureHttpMessageHandler(func)//为httpclient加入httpmessagehandler
            //降级策略
            .AddPolicyHandler
            (
                Policy<HttpResponseMessage>.HandleInner<Exception>().FallbackAsync(fallbackResponse, async f =>
                 {
                     //降级打印异常
                     Console.WriteLine($"服务{cfg.PollyName}开始降级,异常消息：{f.Exception.Message}");
                     //降级之后的数据
                     Console.WriteLine($"服务{cfg.PollyName}降级内容响应：{fallbackResponse.RequestMessage}");
                     await Task.CompletedTask;
                 }
            ))
            //断路器策略
            .AddPolicyHandler
            (
                Policy<HttpResponseMessage>.Handle<TimeoutException>().CircuitBreakerAsync(cfg.CircuitBreakerOpenFallCount, TimeSpan.FromSeconds(cfg.CircuitBreakerDownTime), (ex, ts) =>
                  {
                      Console.WriteLine($"服务{cfg.PollyName}断路器开启，异常消息：{ex.Exception.Message}");
                      Console.WriteLine($"服务{cfg.PollyName}断路器开启时间：{ts.TotalSeconds}s");
                  }, 
                  () => { Console.WriteLine($"服务{cfg.PollyName}断路器关闭"); }, 
                  () => { Console.WriteLine($"服务{cfg.PollyName}断路器半开启(时间控制，自动开关)"); })
            )
            //重试策略
            .AddPolicyHandler
            (
                Policy<HttpResponseMessage>.Handle<Exception>().RetryAsync(cfg.RetryCount)
            )
            //超时策略
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(cfg.TimeoutTime)));

        services.AddSingleton<IHttpService,HttpService>();
        return services;

    }

    private static IHttpClientBuilder ConfigureHttpMessageHandler(this IHttpClientBuilder builder, Func<HttpMessageHandler> func)
    {
        if (func == null || func.Invoke() == null)
        {
            return builder;
        }
        return builder.ConfigurePrimaryHttpMessageHandler(func);//如果有自定义的HttpMessageHandler，则使用自定义的
    }
}
