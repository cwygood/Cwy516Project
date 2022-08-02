using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class User_Info : BaseModel
    {
        /// <summary>
        /// 用户登录账户
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }
        /// <summary>
        /// 禁用截止时间
        /// </summary>
        public DateTime? LockTime { get; set; }
    }
}
