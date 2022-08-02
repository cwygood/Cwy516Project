using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Common.Mq;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly RabbitMqClient _rabbitmqClient;
        private readonly IDbHelper _dbHelper;
        private readonly IOptionsMonitor<EventConfiguration> _options;
        
        public Job(RabbitMqClient rabbitMqClient, IDbHelper dbHelper, IOptionsMonitor<EventConfiguration> options)
        {
            this._rabbitmqClient = rabbitMqClient;
            this._dbHelper = dbHelper;
            this._options = options;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"执行job{context.JobDetail.Key}");
            if (context.JobDetail.Key.Name.Equals("EventBus"))
            {
                //推送到rabbitmq
                var eventLogs = this._dbHelper.QueryAll<EventLog>($"select * from {this._options.CurrentValue.EventBusTableName}");
                foreach(var eventLog in eventLogs)
                {
                    this._rabbitmqClient.PushMessage("eventbus", eventLog.Content, "516project");
                }
                //this._rabbitmqClient.GetMessage("eventbus", "516project");
            }
            return Task.CompletedTask;
        }
    }
}
