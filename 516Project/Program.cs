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
                .AddJsonFile("Config/logging.json", false, true)
                .AddJsonFile("Config/redis.json", false, true)
                .AddJsonFile("Config/jwt.json", false, true)
                .AddJsonFile("Config/rabbitmq.json", false, true)
                .AddJsonFile("Config/polly.json",false,true)
                .AddJsonFile("Config/mysql.json", false, true);//optional:ȱʧ������reloadonchange:�޸���֮�����¼���
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
