using Domain.Models;
using Infrastructure.Common.SnowFlake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Enums;

namespace Infrastructure.EntityFrameworkCore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(EFCoreDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.User_infos.Any())
            {
                return;//存在则不执行
            }
            var userInfos = new User_Info[]
            {
                new User_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "Admin", Name = "管理员", Password = "123456", LockTime = null, Status = UserStatus.Normal },
                new User_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "Cwy", Name = "普通用户", Password = "123456", LockTime = null, Status = UserStatus.Normal },
                new User_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "TestLock", Name = "锁定用户", Password = "123456", LockTime = DateTime.Now.AddMonths(1), Status = UserStatus.Lock },
                new User_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "TestForbid", Name = "禁用用户", Password = "123456", LockTime = null, Status = UserStatus.Forbid }
            };
            foreach(var userInfo in userInfos)
            {
                context.User_infos.Add(userInfo);
            }
            context.SaveChanges();

            var menuInfos = new Menu_Info[]
            {
                new Menu_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "HomePage", Name = "首页", ParentId = 0, CreateTime = DateTime.Now, Status = true },
                new Menu_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "SystemManage", Name = "系统管理", ParentId = 0, CreateTime = DateTime.Now, Status = true },
                new Menu_Info(){ Id = SnowFlake.CreateSnowFlakeId(), Code = "UserManager", Name = "用户管理", ParentId = 0, CreateTime = DateTime.Now, Status = true }
            };
            foreach(var menuInfo in menuInfos)
            {
                context.Menu_Infos.Add(menuInfo);
            }
            context.SaveChanges();

        }
    }
}
