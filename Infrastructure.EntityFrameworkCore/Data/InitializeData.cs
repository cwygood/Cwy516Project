using Infrastructure.EntityFrameworkCore;
using Infrastructure.EntityFrameworkCore.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class InitializeData
{
    public static void Initialize(this IServiceCollection services)
    {
        var dbContext = services.BuildServiceProvider().GetService<EFCoreDbContext>();
        DbInitializer.Initialize(dbContext);
    }
}
