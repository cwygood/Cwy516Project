using Infrastructure.Common.Mq;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mq
{
    public class ReceiveMq:RabbitMqListener
    {
        private readonly ILogger<ReceiveMq> _logger;
        public ReceiveMq(IOptionsMonitor<RabbitMqConfiguration> options, ILogger<ReceiveMq> logger)
            : base(options, logger)
        {
            base.RouteKey = "rabbitmqTest";
            base.ExchangeName = "20210818";
            base.QueueName = "MyTestQueue";
            this._logger = logger;
        }

        public override bool Process(string message)
        {
            this._logger.LogInformation(message);
            return true;
        }
    }
}
