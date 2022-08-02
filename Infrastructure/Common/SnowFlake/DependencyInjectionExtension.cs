using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Yitter.IdGenerator;

public static partial class DependencyInjectionExtension
{
    public static void AddSnowFlake(this IServiceCollection services)
    {
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions() { WorkerIdBitLength = 6 });
    }
}
