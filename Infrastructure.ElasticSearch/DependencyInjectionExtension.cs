using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<ElasticSearchConfiguration>(section);
        services.AddSingleton<IESServer, ESServer>();
        return services;
    }
}
