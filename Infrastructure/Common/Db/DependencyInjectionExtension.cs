using Infrastructure.Common.Db;
using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfigurationSection section)
    {
        services.AddSingleton<CusDbContext>();
        services.Configure<DatabaseConfiguration>(section);
        return services;
    }
}
