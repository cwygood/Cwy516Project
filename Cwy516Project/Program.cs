using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cwy516Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json")
                .AddJsonFile("Config/logging.json", false, true)
                .AddJsonFile("Config/redis.json", false, true)
                .AddJsonFile("Config/jwt.json", false, true)
                .AddJsonFile("Config/rabbitmq.json", false, true)
                .AddJsonFile("Config/polly.json",false,true)
                .AddJsonFile("Config/mongodb.json",false,true)
                .AddJsonFile("Config/jaeger.json",false,true)
                .AddJsonFile("Config/consul.json",false,true)
                .AddJsonFile("Config/ocelot.json",false,true)
                .AddJsonFile("Config/identityserver.json",false,true)
                .AddJsonFile("Config/elasticsearch.json",false,true)
                .AddJsonFile("Config/kafka.json",false,true)
                .AddJsonFile("Config/eventbus.json",false,true)
                .AddJsonFile("Config/mysql.json", false, true);//optional:缺失不报错，reloadonchange:修改了之后重新加载
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.ClearProviders();
                builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                builder.AddLog4Net("Config/log4net.xml", true);
                builder.AddDebug();
                builder.AddConsole();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
