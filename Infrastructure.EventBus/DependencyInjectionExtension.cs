using Infrastructure.Configurations;
using Infrastructure.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<EventConfiguration>(section);
        services.AddSingleton<Domain.Interfaces.IEventFactory, EventFacotry>();
        return services;
    }
}
