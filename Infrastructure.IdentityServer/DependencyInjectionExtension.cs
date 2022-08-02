using Infrastructure.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddMyIdentityServer(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<IdentityServerConfiguration>(section);
        var cfg = section.Get<IdentityServerConfiguration>();
        services.AddIdentityServer()
                .AddDeveloperSigningCredential() //Identity Server4将创建一个开发人员签名密钥，该文件名为tempkey.jwk，只适合单机，生产环境使用AddSigningCredential()
                //.AddTestUsers(cfg.Users.ToList()) //测试专用用户
                .AddInMemoryClients(cfg.Clients)
                .AddInMemoryApiResources(cfg.ApiResources)
                .AddInMemoryApiScopes(cfg.ApiScopes)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();//自定义验证
        return services;
    }

    public static void UseMyIdentityServer(this IApplicationBuilder app)
    {
        app.UseIdentityServer();
    }
}
