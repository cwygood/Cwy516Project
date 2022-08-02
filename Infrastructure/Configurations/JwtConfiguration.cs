using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class JwtConfiguration
    {
        /// <summary>
        /// 发布者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string SigningKey { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expires { get; set; }
    }
}
