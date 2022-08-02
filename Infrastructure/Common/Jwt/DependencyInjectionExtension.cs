using Domain.Interfaces;
using Infrastructure.Common.Jwt;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

public static partial class DependencyInjectionExtension
{
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfigurationSection section)
    {
        var jwtCfg = section.Get<JwtConfiguration>();
        //jwt 身份认证
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    RoleClaimType="Roles",
                    ValidIssuer = jwtCfg.Issuer,//发布
                    ValidAudience = jwtCfg.Audience,//订阅
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,//是否校验有效期
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtCfg.SigningKey)),
                    //缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.Configure<JwtConfiguration>(section);
        services.AddSingleton<IJwtToken>(new JwtToken(jwtCfg));
        return services;
    }
}
