using Infrastructure.Configurations;
using Infrastructure.Jaeger;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddJaeger(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<JaegerConfiguration>(section);
        var cfg = section.Get<JaegerConfiguration>();
        var host = string.IsNullOrWhiteSpace(cfg.Host) ? UdpSender.DefaultAgentUdpHost : cfg.Host;
        var port = cfg.Port <= 100 ? UdpSender.DefaultAgentUdpCompactPort : cfg.Port;
        var maxPacketSize = cfg.MaxPacketSize < 0 ? 0 : cfg.MaxPacketSize;
        services.AddOpenTracing();
        services.AddSingleton<ITracer>(provider =>
        {
            string serviceName = services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>().ApplicationName;
            
            var loggerFactory = provider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
            Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory).RegisterSenderFactory<ThriftSenderFactory>();
            var senderConfiguration = new Jaeger.Configuration.SenderConfiguration(loggerFactory);
            if (!string.IsNullOrWhiteSpace(cfg.Endpoint))
            {
                senderConfiguration.WithEndpoint(cfg.Endpoint);
            }
            else
            {
                senderConfiguration.WithAgentHost(host).WithAgentPort(port);//该方法暂时无效，未找到原因，怀疑和linux的udp端口有关（todo）
            }
            

            var samplerConfiguration = new Jaeger.Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type).WithParam(1);
            var reporterConfiguration = new Jaeger.Configuration.ReporterConfiguration(loggerFactory)
                .WithLogSpans(true)
                .WithSender(senderConfiguration);

            ITracer tracer = tracer = new Jaeger.Configuration(serviceName, loggerFactory)
                   .WithSampler(samplerConfiguration)
                   .WithReporter(reporterConfiguration)
                   .GetTracer();

            if (!OpenTracing.Util.GlobalTracer.IsRegistered())
            {
                OpenTracing.Util.GlobalTracer.Register(tracer);
            }


            return tracer;
        });
        
        return services;
    }

    public static IApplicationBuilder UseJaeger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JaegerMiddleware>();
    }
}
