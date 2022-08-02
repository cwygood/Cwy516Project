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

                #region ��ȡ�����ļ�
                //�����ȡxml�ļ���
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //����xmlע�ͣ��ڶ��������Ƿ�������������ע�ͣ�Ĭ��false
                swaggerOptions.IncludeXmlComments(xmlPath, true);
                #endregion

                #region ����Swagger��֤
                //���һ�������ȫ�ְ�ȫ��Ϣ����AddSecurityDefinition����ָ���ķ�������һ�¼��ɣ�Bearer��
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
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ���·�����Bearer {token} ���ɣ�ע������֮���пո�",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });

                #endregion
            });
            services.AddCors(optons =>
            {
                optons.AddPolicy("cors",builder =>
                {
                    builder.SetIsOriginAllowed(f => true)
                            //AllowAnyOrigin()    //���ܺ�AllowCredentialsͬʱʹ��
                            //.WithOrigins(new string[] { "http://localhost:8080" })  //����ĳЩ��ַ���Կ���
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
                    //ʹ���շ���ʽ������ĸСд
                    jsonOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //����ѭ������
                    jsonOptions.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    //��ֵ����
                    jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                    //����ʱ���ʽ
                    jsonOptions.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss:fff";
                    //���ر�׼ʱ��
                    jsonOptions.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    //���ڸ�ʽת��
                    jsonOptions.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                });
            services.AddMediatR(typeof(BaseCommand).Assembly);//�������handler ��Ӧ�ĳ���
            services.AddMySqlRepositories();//ע��ֿ�ӿ�
            services.AddDbContext(Configuration.GetSection("DataBase"));//������ݿ�
#if EF
            services.AddEFCoreDbContext(Configuration.GetSection("Database"));//�����ļ�������Ǩ�Ƴ���
            services.Initialize();//��ʼ������
#elif Dapper
            services.AddDapper();
#else
            services.AddMyOrm();
#endif
            services.AddCache(Configuration.GetSection("Redis"));//���redis
            services.AddJwt(Configuration.GetSection("Jwt"));//���jwt��֤
            var useIdentityServer = Configuration.GetValue<bool>("UseIdentity");
            if (useIdentityServer)
            {
                services.AddMyIdentityServer(Configuration.GetSection("IdentityServer"));
            }
            services.AddSnowFlake();//ѩ���㷨
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
            //identityserver��֤
            var useIdentityServer = Configuration.GetValue<bool>("UseIdentity");
            if (useIdentityServer)
            {
                app.UseMyIdentityServer();
            }
            app.UseJaeger();
            app.UseRouting();
            app.UseCors("cors");
            //jwt��֤
            app.UseAuthentication();
            app.UseAuthorization();//������UseRouting��UseEndpoints֮��

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
