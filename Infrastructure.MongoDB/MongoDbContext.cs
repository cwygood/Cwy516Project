using Domain.Models;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.MongoDB
{
    
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IOptionsMonitor<MongoDbConfiguration> options)
        {
            var client = new MongoClient(options.CurrentValue.Connectionstring);
            this._database = client.GetDatabase(options.CurrentValue.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var className = typeof(T).Name;
            return this._database.GetCollection<T>(className);
        }

        public IEnumerable<T> Query<T>(Expression<Func<T,bool>> expression) where T : class
        {
            return this.GetCollection<T>().Find(expression).ToList();
        }

        public void Add<T>(T t) where T : class
        {
            this.GetCollection<T>().InsertOne(t);
        }

        public void Update<T>(T t)where T : BaseModel
        {
            var builder = Builders<T>.Update;
            var update = builder.Set(f => f.Id, t.Id);
            this.GetCollection<T>().UpdateOne(f => f.Id == t.Id, update);
        }
    }
}
