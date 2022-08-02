using Confluent.Kafka;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<KafkaConfiguration>(section);
        services.AddSingleton<IKafkaHelper, KafkaHelper>();
        return services;
    }
}
