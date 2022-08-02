using Domain.Interfaces;
using Infrastructure.Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddDapper(this IServiceCollection services)
    {
        services.AddSingleton<DapperDbContext>();
        services.AddScoped<IDbHelper, DapperHelper>();
        return services;
    }
}
