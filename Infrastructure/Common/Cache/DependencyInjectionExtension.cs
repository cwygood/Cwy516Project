using Domain.Interfaces;
using Infrastructure.Common.Cache;
using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddCache(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<RedisConfiguration>(section);
        services.AddSingleton<IRedisCache, RedisCache>();
        services.AddMemoryCache();
        return services;
    }
}