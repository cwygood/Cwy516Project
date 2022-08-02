using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.EntityFrameworkCore
{
    public class EFCoreDbHelper : IDbHelper
    {
        private readonly EFCoreDbContext _context;
        public EFCoreDbHelper(EFCoreDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<T> QueryAll<T>() where T : class
        {
            var dbSet = this._context.Set<T>();
            return dbSet;
        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class
        {
            var dbSet = this._context.Set<T>();
            var entitys = dbSet.Where(expression);
            if (!entitys.Any())
            {
                return null;
            }
            return entitys;
        }

        public T QueryFirstOrDefault<T>(Expression<Func<T,bool>> expression) where T : class
        {
            var dbSet = this._context.Set<T>();
            var entitys = dbSet.Where(expression);
            if (!entitys.Any())
            {
                return null;
            }
            return entitys.FirstOrDefault();
        }

        #region 忽略
        [Obsolete]
        public IEnumerable<T> QueryAll<T>(string sql)
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public IEnumerable<T> QueryAll<T>(string sql, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public bool Excute(string sql, object param)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Add<T>(T t) where T : class
        {
            this._context.Add(t);
            this._context.SaveChanges();
        }
    }
}
