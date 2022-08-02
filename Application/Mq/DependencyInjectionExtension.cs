using Application.Mq;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddCustomerMq(this IServiceCollection services)
    {
        //services.AddHostedService<GetUserReceiveFromMq>();
        //services.AddHostedService<ReceiveMq>();
        return services;
    }
}
