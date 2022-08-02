using Domain.Interfaces;
using Infrastructure.MyOrm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddMyOrm(this IServiceCollection services)
    {
        services.AddSingleton<MyOrmDbContext>();
        services.AddScoped<IDbHelper, MyOrmDbHelper>();
        return services;
    }
}