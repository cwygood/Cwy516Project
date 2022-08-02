using Cwy516Project;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Polly;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz.Spi;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Cwy516TestProject
{
    public class CustomTestFixture : WebApplicationFactory<Startup>
    {
        public MockHttpMessageHandler handler { get; set; }
        public IJobFactory jobFactory { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Dev");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(HttpClient));
                var config = services.BuildServiceProvider().GetService<IConfiguration>();
                jobFactory = services.BuildServiceProvider().GetService<IJobFactory>();
                
                var section = config.GetSection("Polly");
                services.AddPollyHttpClient(section, () => handler);

                services.AddQuartz();
            });
        }

    }
}
