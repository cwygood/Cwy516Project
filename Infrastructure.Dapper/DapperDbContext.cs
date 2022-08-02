using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Dapper
{
    public class DapperDbContext
    {
        private readonly string _connectionString;
        private readonly string _readConnectionString;
        public DapperDbContext(IOptions<DatabaseConfiguration> mySqlOptions)
        {
            this._connectionString = mySqlOptions.Value.ConnectionString;
            this._readConnectionString = mySqlOptions.Value.ReadConnectionString;
        }
        /// <summary>
        /// 写库连接
        /// </summary>
        /// <returns></returns>
        public MySqlConnection DbConnection()
        {
            if (string.IsNullOrWhiteSpace(this._connectionString))
            {
                throw new Exception("ConnectionString is Null");
            }
            else
            {
                return new MySqlConnection(this._connectionString);
            }
        }
        /// <summary>
        /// 读库连接
        /// </summary>
        /// <returns></returns>
        public MySqlConnection ReadDbConnection()
        {
            if (string.IsNullOrWhiteSpace(this._readConnectionString))
            {
                throw new Exception("ReadConnectionString is Null");
            }
            else
            {
                return new MySqlConnection(this._readConnectionString);
            }
        }
    }
}
