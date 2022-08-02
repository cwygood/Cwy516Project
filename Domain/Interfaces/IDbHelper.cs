using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces
{
    public interface IDbHelper
    {
        IEnumerable<T> QueryAll<T>() where T : class;
        IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class;
        T QueryFirstOrDefault<T>(Expression<Func<T, bool>> expression) where T : class;

        IEnumerable<T> QueryAll<T>(string sql);
        T QueryFirstOrDefault<T>(string sql, object param);
        IEnumerable<T> QueryAll<T>(string sql, Dictionary<string,object> param);
        bool Excute(string sql, object param);
        void Add<T>(T t) where T : class;
    }
}
