using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IRedisCache : IDisposable
    {
        IDatabase GetDatabase(int type);
        IServer GetServer(int type);
        ISubscriber GetSubscriber();
        bool SetString(string key, string value, TimeSpan? span = null, string prefix = "");
        string GetString(string key, string prefix = "");
    }
}
