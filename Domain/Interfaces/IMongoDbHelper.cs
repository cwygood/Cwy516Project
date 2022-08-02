using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces
{
    public interface IMongoDbHelper
    {
        void Add<T>(T t) where T : class;
        IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class;
    }
}
