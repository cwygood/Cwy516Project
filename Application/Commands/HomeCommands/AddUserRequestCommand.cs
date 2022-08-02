using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.HomeCommands
{
    public class AddUserRequestCommand : BaseCommand, IRequest<AddUserResponseCommand>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
