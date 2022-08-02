using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IUserRepository : IBaseRepository
    {
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<User_Info> GetAllUser();
        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User_Info GetUserById(long id);
        /// <summary>
        /// 通过code获取用户信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        User_Info GetUserInfo(string userCode);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool AddUser(User_Info userInfo);
        /// <summary>
        /// 新增Mongodb集合
        /// </summary>
        /// <param name="userInfo"></param>
        void AddMongoDb(User_Info userInfo);
    }
}
