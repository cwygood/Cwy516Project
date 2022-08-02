using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum UserStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 禁用
        /// </summary>
        Forbid = 1,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock = 2
    }
}
