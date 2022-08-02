using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Mq
{
    public class RabbitMqClient
    {
        private readonly ILogger<RabbitMqClient> _logger;
        private readonly IModel _sendChannel;
        private readonly Dictionary<string, IModel> _receiveChannels;
        public RabbitMqClient(IOptionsMonitor<RabbitMqConfiguration> options, ILogger<RabbitMqClient> logger)
        {
            this._logger = logger;
            this._receiveChannels = new Dictionary<string, IModel>();
            try
            {
                if (options.CurrentValue.Cluster != null)
                {
                    var cluster = options.CurrentValue.Cluster;
                    
                    var sendFactory = new ConnectionFactory()
                    {
                        HostName = cluster.Producter.Host,
                        UserName = cluster.Producter.User,
                        Password = cluster.Producter.Password,
                        Port = cluster.Producter.Port
                    };
                    this._sendChannel = sendFactory.CreateConnection().CreateModel();
                    foreach(var consumer in cluster.Consumer)
                    {
                        var receiveFactory = new ConnectionFactory()
                        {
                            HostName = consumer.Host,
                            UserName = consumer.User,
                            Password = consumer.Password,
                            Port = consumer.Port
                        };
                        this._receiveChannels.Add(consumer.Host + "_" + consumer.Port, receiveFactory.CreateConnection().CreateModel());
                    }
                }
                else
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = options.CurrentValue.HostInfo.Host,
                        UserName = options.CurrentValue.HostInfo.User,
                        Password = options.CurrentValue.HostInfo.Password,
                        Port = options.CurrentValue.HostInfo.Port
                    };
                    var connection = factory.CreateConnection();
                    this._sendChannel = connection.CreateModel();
                    this._receiveChannels.Add(options.CurrentValue.HostInfo.Host + "_" + options.CurrentValue.HostInfo.Port, connection.CreateModel());
                }
                
            }
            catch (Exception ex)
            {
                this._logger.LogError(-1, ex, "RabbitMqClient Init Fail");
            }
        }
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="message">消息内容</param>
        /// <param name="exchangeName"></param>
        public virtual void PushMessage(string routeKey, object message, string exchangeName)
        {
            this._logger.LogInformation($"PushMessage routeKey:{routeKey}");
            this._sendChannel.ExchangeDeclare(exchange: exchangeName, type: "topic");
            //this._channel.QueueDeclare(
            //    queue: routeKey,//是exchange知道exchange应该发到哪个队列上
            //    durable: false,//是否持久化，保存到磁盘
            //    exclusive: false,//设置是否排他。true排他的。如果一个队列声明为排他队列，该队列仅对首次声明它的连接可见，并在连接断开时自动删除。
            //    /*
            //     * 排它是基于连接可见的，同一个连接不同信道是可以访问同一连接创建的排它队列，“首次”是指如果一个连接已经声明了一个排他队列，
            //     * 其他连接是不允许建立同名的排他队列，即使这个队列是持久化的，一旦连接关闭或者客户端退出，该排它队列会被自动删除，这种队列适用于一个客户端同时发送与接口消息的场景。
            //     */
            //    autoDelete: false,//设置是否自动删除。true是自动删除。自动删除的前提是：致少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开 时，才会自动删除,没有消费者客户端与这个队列连接时，不会自动删除这个队列
            //    arguments: null//设置队列的其他参数
            //    );
            var msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            this._sendChannel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: null, body: body);
        }

        public virtual void GetMessage(string routeKey, string exchangeName)
        {
            Parallel.ForEach(this._receiveChannels, channel =>
            {
                channel.Value.ExchangeDeclare(exchangeName, "topic");
                channel.Value.QueueDeclare("queue", true, false, false);
                channel.Value.QueueBind("queue", exchangeName, routeKey);
                //公平分发,不要同一时间给一个工作者发送多于一个消息 
                channel.Value.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel.Value);
                consumer.Received += (model, e) =>
                {
                    var body = e.Body.ToArray();
                    var msg = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"{channel.Key}消息：{msg}");
                    channel.Value.BasicAck(e.DeliveryTag, false);
                };
                //指定从哪个消费者从哪个通道获取消息，并指明自动确认的机制 //参数1：队列名，参数2：确认机制，true表示自动确认，false代表手动确认，参数3：消费者 
                var tag = channel.Value.BasicConsume("queue", false, consumer);
            });
        }

        private void Consumber_Received(object sender, BasicDeliverEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
