using Infrastructure.Common.Mq;
using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<RabbitMqConfiguration>(section);
        services.AddSingleton<RabbitMqClient>();
        return services;
    }
}
