using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<MongoDbConfiguration>(section);//配置mongodb
        services.AddSingleton<MongoDbContext>();
        services.AddSingleton<IMongoDbHelper, MongoDbHelper>();
        return services;
    }
}
