using Dapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Dapper
{
    public class DapperHelper : IDbHelper
    {
        private readonly DapperDbContext _context;
        public DapperHelper(DapperDbContext context)
        {
            this._context = context;
        }

        #region Ignore
        [Obsolete]
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public IEnumerable<T> QueryAll<T>() where T : class
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public T QueryFirstOrDefault<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public IEnumerable<T> QueryAll<T>(string sql, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public void Add<T>(T t) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion

        public IEnumerable<T> QueryAll<T>(string sql)
        {
            return this._context.ReadDbConnection().Query<T>(sql);
        }
        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            return this._context.ReadDbConnection().QueryFirstOrDefault<T>(sql, param);
        }
        
        public bool Excute(string sql, object param)
        {
            return this._context.DbConnection().Execute(sql, param) > 0;
        }
        
    }
}
