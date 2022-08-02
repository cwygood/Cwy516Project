using Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Common.Mq
{
    public class RabbitMqListener : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private ILogger<RabbitMqListener> _logger;
        public RabbitMqListener(IOptionsMonitor<RabbitMqConfiguration> options, ILogger<RabbitMqListener> logger)
        {
            try
            {
                //rabbitmq网络连接使用的是15672（http），但是此处连接的端口必须是5672（amqp)
                var factory = new ConnectionFactory()
                {
                    HostName = options.CurrentValue.HostInfo.Host,
                    UserName = options.CurrentValue.HostInfo.User,
                    Password = options.CurrentValue.HostInfo.Password,
                    Port = options.CurrentValue.HostInfo.Port
                };
                this._logger = logger;
                this._connection = factory.CreateConnection();
                this._channel = this._connection.CreateModel();
            }
            catch (Exception ex)
            {
                this._logger.LogError(-1, ex, "RabbitMqListener Init Fail");
            }
        }
        public virtual bool Process(string message)
        {
            return true;
        }
        public string RouteKey { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"RabbitMqListener RouteKey:{RouteKey}");
            this._channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(this._channel);
            this._channel.ExchangeDeclare(exchange: ExchangeName, "topic");
            this._channel.QueueDeclare(queue: QueueName, false, false, false, null);
            this._channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RouteKey, null);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                this._logger.LogInformation($"RabbitMqListener Message:{message}");
                var result = Process(message);
                if (result)
                {
                    this._channel.BasicAck(eventArgs.DeliveryTag, true);
                }
            };
            this._channel.BasicConsume(this.QueueName, false, consumer);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this._connection.Close();
            return Task.CompletedTask;
        }
    }
}
