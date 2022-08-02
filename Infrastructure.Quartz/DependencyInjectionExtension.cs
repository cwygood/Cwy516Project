using Infrastructure.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.AddTransient<Job>();
        services.AddSingleton<IJobFactory, CustomerJobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<TaskPoolManager>();

        return services;
    }
}
