using Domain.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.MyOrm
{
    public class MyOrmDbHelper : IDbHelper
    {
        private readonly MyOrmDbContext _context;
        public MyOrmDbHelper(MyOrmDbContext context)
        {
            this._context = context;
        }
        [Obsolete]
        public void Add<T>(T t) where T : class
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public bool Excute(string sql, object param)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryAll<T>(string sql, Dictionary<string,object> param = null)
        {
            List<T> datas = new List<T>();
            var conn = this._context.DbConnection();
            conn.Open();
            var cmd = new MySqlCommand(sql, conn);
            if (param != null && param.Count > 0)
            {
                foreach (var para in param)
                {
                    cmd.Parameters.AddWithValue(para.Key, para.Value);
                }
            }
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    var objT = Activator.CreateInstance(typeof(T));
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        prop.SetValue(objT, dr[prop.Name] == DBNull.Value ? null : dr[prop.Name]);
                    }
                    datas.Add((T)objT);
                }
            }
            conn.Close();
            conn.Dispose();
            return datas;
        }
        [Obsolete]
        public IEnumerable<T> QueryAll<T>() where T : class
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public IEnumerable<T> QueryAll<T>(string sql)
        {
            throw new NotImplementedException();
        }
        public T QueryFirstOrDefault<T>(string sql, Dictionary<string,object> param) where T : class
        {
            return QueryAll<T>(sql, param).FirstOrDefault();
        }
        [Obsolete]
        public T QueryFirstOrDefault<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            throw new NotImplementedException();
        }
    }
}
