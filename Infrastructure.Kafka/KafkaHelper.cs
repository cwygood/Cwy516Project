using Confluent.Kafka;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Kafka
{
    public class KafkaHelper : IKafkaHelper
    {
        private readonly IOptionsMonitor<KafkaConfiguration> _options;
        public KafkaHelper(IOptionsMonitor<KafkaConfiguration> options)
        {
            this._options = options;
        }

        public async Task<string> PublishAsync<T>(string topic, T data) where T : class
        {
            var config = new ProducerConfig
            {
                BootstrapServers = this._options.CurrentValue.Url//集群的话，有多个地址，用逗号隔开
            };
            using(var producer=new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    var res = await producer.ProduceAsync(topic, new Message<string, string>()
                    {
                        Key = Guid.NewGuid().ToString("N"),
                        Value = JsonConvert.SerializeObject(data)
                    });
                    //Console.WriteLine($"Delivered [{res.Value}] To Partition [{res.TopicPartition}] Offset [{res.TopicPartitionOffset}]");
                    return res.Value;
                }
                catch(ProduceException<string,string> ex)
                {
                    Console.WriteLine($"Deelivered Failed:{ex.Error.Reason}");
                }
            }
            return "";
        }

        public async Task<T> SubscribeAsync<T>(IEnumerable<string> topics, CancellationToken cancellationToken = default) where T : class
        {
            var config = new ConsumerConfig()
            {
                GroupId = "consumer",
                BootstrapServers = this._options.CurrentValue.Url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using(var consumer=new ConsumerBuilder<Ignore, string>(config)
                .SetErrorHandler((ic, ex) => { Console.WriteLine($"Kafka Error:{ex.Reason}"); })
                .SetStatisticsHandler((ic, msg) => { Console.WriteLine($"-{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}>消息监听中：{msg}"); })
                .SetPartitionsAssignedHandler((ic,partitions)=> 
                {
                    string partitionStr = string.Join(",", partitions);
                    Console.WriteLine($"分配的Kafka分区：{partitionStr}");
                })
                .SetPartitionsRevokedHandler((ic, partitions) => 
                {
                    string partitionStr = string.Join(",", partitions);
                    Console.WriteLine($"回收的Kafka分区：{partitionStr}");
                })
                .Build())
            {
                consumer.Subscribe(topics);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cancellationToken);
                            if (consumeResult.IsPartitionEOF)
                            {
                                Console.WriteLine(
                                    $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
                                break;
                            }
                            Console.WriteLine($"Receive Message At {consumeResult.TopicPartitionOffset} : {consumeResult.Message.Value}");
                            try
                            {
                                var msg = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);
                                return await Task.FromResult(msg);
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine($"Msg Convert Failed:{consumeResult.Message.Value}");
                            }
                            //return await Task.FromResult<T>(null);
                        }
                        catch(ConsumeException ex)
                        {
                            Console.WriteLine($"Consume Error:{ex.Error.Reason}");
                        }
                    }
                }
                catch(OperationCanceledException ex)
                {
                    Console.WriteLine("Close Consumer...");
                    consumer.Close();
                }
            }
            return await Task.FromResult<T>(null);
        }
    }
}
