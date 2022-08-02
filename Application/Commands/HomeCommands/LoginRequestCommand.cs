using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.HomeCommands
{
    public class LoginRequestCommand : BaseCommand, IRequest<LoginResponseCommand>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
        /// <summary>
        /// 验证码缓存标识
        /// </summary>
        public string ValidateKey { get; set; }
    }
}
