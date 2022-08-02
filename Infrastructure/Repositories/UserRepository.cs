using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Common.Db;
using Infrastructure.Common.SnowFlake;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Yitter.IdGenerator;

namespace Infrastructure.Repositories
{
    public class UserRepository : CusDbContext, IUserRepository
    {
        public UserRepository(IDbHelper context, IMongoDbHelper mongoDbHelper) : base(context, mongoDbHelper)
        {

        }
        public IEnumerable<User_Info> GetAllUser()
        {
            //return this.QueryAll<User_Info>();//EF查询
            return this.QueryAll<User_Info>("select * from user_info");
            //return this.QueryAdoAsync<User_Info>("select * from user_info ", null);//ADO.net 查询
        }

        public User_Info GetUserById(long id)
        {
            return this.QueryFirstOrDefault<User_Info>("select * from user_info where id=@id", new { id = id });
            //return this.QueryFirstOrDefault<User_Info>(f => f.Id == id);//EF查询
            //return this.QueryFirstOrDefaultAsync<User_Info>("select * from user_info where id=@id", new Dictionary<string, object>() { { "@id", id } });//ADO.net 查询
        }

        public User_Info GetUserInfo(string userCode)
        {
            return this.QueryFirstOrDefault<User_Info>("select id,code,name,password,status,locktime from user_info where code=@code", new { code = userCode });
        }

        public bool AddUser(User_Info userInfo)
        {
            return this.Excute(@" insert into user_info(id,code,name,password,status) 
                                            values(@id,@code,@name,@password,@status)",
                                            new
                                            {
                                                id = SnowFlake.CreateSnowFlakeId(),
                                                code = userInfo.Code,
                                                name = userInfo.Name,
                                                password = userInfo.Password,
                                                status = UserStatus.Normal
                                            });
        }
        public void AddMongoDb(User_Info userInfo)
        {
            if (userInfo.Id == 0)
            {
                userInfo.Id = SnowFlake.CreateSnowFlakeId();
            }
            this.AddMongoDb<User_Info>(userInfo);
        }
    }
}
