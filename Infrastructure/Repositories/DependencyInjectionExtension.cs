using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddMySqlRepositories(this IServiceCollection services)
    {
        //需要添加Scrutor nuget包
        services.Scan(scan => scan
        .FromAssemblyOf<UserRepository>()
        .AddClasses(classes => classes.AssignableTo<IBaseRepository>())
        .AsImplementedInterfaces()
        .WithTransientLifetime());

        //var list = services.Where(x => x.ServiceType.Namespace.Equals("ScrutorTest", StringComparison.OrdinalIgnoreCase)).ToList();

        //foreach (var item in services)
        //{
        //    Console.WriteLine($"{item.Lifetime},{item.ImplementationType},{item.ServiceType}");
        //}
        return services;
    }
}
