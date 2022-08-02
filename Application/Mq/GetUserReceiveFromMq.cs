using Infrastructure.Common.Mq;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mq
{
    public class GetUserReceiveFromMq : RabbitMqListener
    {
        private readonly ILogger<GetUserReceiveFromMq> _logger;
        public GetUserReceiveFromMq(IOptionsMonitor<RabbitMqConfiguration> options, ILogger<GetUserReceiveFromMq> logger)
            : base(options, logger)
        {
            base.RouteKey = "user.test";
            base.QueueName = "Test";
            base.ExchangeName = "516project";
            this._logger = logger;
        }

        public override bool Process(string message)
        {
            var msg = JToken.Parse(message);
            if (msg == null)
            {
                return false;
            }
            this._logger.LogInformation($"GetUserReceiveFromMq Process Message:{msg}");
            return true;
        }
    }
}
