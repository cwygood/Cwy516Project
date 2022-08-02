using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IKafkaHelper
    {
        Task<string> PublishAsync<T>(string topic, T data) where T : class;
        Task<T> SubscribeAsync<T>(IEnumerable<string> topics, CancellationToken cancellationToken = default) where T : class;
    }
}
