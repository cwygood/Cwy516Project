using Application.Commands;
using Application.Filters;
using Application.Validations;
using Autofac;
using FluentValidation.AspNetCore;
using Infrastructure.Configurations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cwy516Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Demo",
                    Version = "V1",
                    Description = "516Project WebAPI",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "51",
                        Email = "test@qq.com",
                        Url = new System.Uri("https://home.cnblogs.com/u/cwy51blogs/")
                    }
                });

                #region 读取配置文件
                //反射获取xml文件名
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //启用xml注释，第二个参数是否启动控制器的注释，默认false
                swaggerOptions.IncludeXmlComments(xmlPath, true);
                #endregion

                #region 启用Swagger验证
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，Bearer。
                var security = new OpenApiSecurityRequirement();
                security.Add(new OpenApiSecurityScheme()
                {
                    Scheme = "Bearer",
                    Name = "Bearer",
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                }, new string[] { });
                swaggerOptions.AddSecurityRequirement(security);
                swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

                #endregion
            });
            services.AddCors(optons =>
            {
                optons.AddPolicy("cors",builder =>
                {
                    builder.SetIsOriginAllowed(f => true)
                            //AllowAnyOrigin()    //不能和AllowCredentials同时使用
                            //.WithOrigins(new string[] { "http://localhost:8080" })  //设置某些地址可以跨域
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                });
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<UrlFilter>();
            })
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<UserInfoValidation>())
                .AddNewtonsoftJson(jsonOptions =>
                {
                    //使用驼峰样式，首字母小写
                    jsonOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //忽略循环引用
                    jsonOptions.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    //空值数据
                    jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                    //设置时间格式
                    jsonOptions.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss:fff";
                    //返回标准时区
                    jsonOptions.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    //日期格式转换
                    jsonOptions.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                });
            services.AddMediatR(typeof(BaseCommand).Assembly);//必须包含handler 对应的程序集
            services.AddMySqlRepositories();//注册仓库接口
            services.AddDbContext(Configuration.GetSection("DataBase"));//添加数据库
#if EF
            services.AddEFCoreDbContext(Configuration.GetSection("Database"));//配置文件错误导致迁移出错
            services.Initialize();//初始化数据
#elif Dapper
            services.AddDapper();
#else
            services.AddMyOrm();
#endif
            services.AddCache(Configuration.GetSection("Redis"));//添加redis
            services.AddJwt(Configuration.GetSection("Jwt"));//添加jwt验证
            var useIdentityServer = Configuration.GetValue<bool>("UseIdentity");
            if (useIdentityServer)
            {
                services.AddMyIdentityServer(Configuration.GetSection("IdentityServer"));
            }
            services.AddSnowFlake();//雪花算法
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddPollyHttpClient(Configuration.GetSection("Polly"));
            services.AddQuartz();

#if Linux
            services.AddRabbitMq(Configuration.GetSection("RabbitMq"));
            services.AddCustomerMq();
            services.AddMongoDB(Configuration.GetSection("MongoDB"));
            services.AddJaeger(Configuration.GetSection("Jaeger"));
            services.AddConsul(Configuration.GetSection("Consul"));
            services.AddMyOcelot();
            services.AddElasticSearch(Configuration.GetSection("ElasticSearch"));
            services.AddKafka(Configuration.GetSection("Kafka"));
            services.AddEventBus(Configuration.GetSection("EventBus"));
#endif
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<Test>().As<ITest>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger()
                    .UseSwaggerUI(swaggerOptions =>
                    {
                        swaggerOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo V1");
                    });
                app.UseDeveloperExceptionPage();
            }
            //identityserver认证
            var useIdentityServer = Configuration.GetValue<bool>("UseIdentity");
            if (useIdentityServer)
            {
                app.UseMyIdentityServer();
            }
            app.UseJaeger();
            app.UseRouting();
            app.UseCors("cors");
            //jwt认证
            app.UseAuthentication();
            app.UseAuthorization();//必须在UseRouting和UseEndpoints之间

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
#if Linux
            app.UseConsul(Configuration.GetSection("Consul"), lifetime, Configuration);
            app.UseMyOcelot();
#endif
        }
    }
}
