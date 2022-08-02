using Consul;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddConsul(this IServiceCollection service, IConfigurationSection section)
    {
        service.Configure<ConsulConfiguration>(section);
        service.AddSingleton<IConsulHelper, ConsulHelper>();
        return service;
    }
    public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfigurationSection section, IHostApplicationLifetime lifetime, IConfiguration configuration)
    {
        var config = section.Get<ConsulConfiguration>();
        var consulClient = new ConsulClient(c =>
          {
              c.Address = new Uri(config.ConsulAddress);
          });
        if (config.ServicePort <= 0)
        {
            var strUrls = configuration.GetValue<string>("urls");
            config.ServicePort = Convert.ToInt32(strUrls.Substring(strUrls.LastIndexOf(":") + 1));
        }
        var registration = new AgentServiceRegistration()
        {
            ID = Guid.NewGuid().ToString("N"),//服务实例唯一id
            Name = config.ServiceName,//服务名称
            Address = config.ServiceHost,//服务IP
            Port = config.ServicePort,//服务端口，可在运行时传入，此参数以配置文件为主，配置文件存在，则不再使用传入的数据
            Check = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                HTTP = $"http://{config.ServiceHost}:{config.ServicePort}/{config.HealthCheck}",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)//超时时间
            }
        };
        //服务注册
        consulClient.Agent.ServiceRegister(registration).Wait();

        lifetime.ApplicationStopping.Register(() =>
        {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });

        //程序启动获取服务器列表
        var consulHelper = app.ApplicationServices.GetRequiredService<IConsulHelper>();
        consulHelper.GetService();
        return app;
    }
}
