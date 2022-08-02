using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.MongoDB
{
    public class MongoDbHelper : IMongoDbHelper
    {
        private readonly MongoDbContext _context;
        public MongoDbHelper(MongoDbContext context)
        {
            this._context = context;
        }

        public void Add<T>(T t) where T : class
        {
            this._context.Add(t);
        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return this._context.Query<T>(expression);
        }
        
    }
}
