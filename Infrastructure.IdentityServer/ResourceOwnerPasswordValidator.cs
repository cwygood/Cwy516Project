using Domain.Repositories;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;
        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var password = context.Password;
                var userInfo = this._userRepository.GetUserInfo(userName);
                if (userInfo == null)
                {
                    throw new Exception("登录失败，用户名不存在");
                }
                if (!string.Equals(userInfo.Password, password, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("密码错误");
                }
                //todo：获取用户角色
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,$"{userName}")
                };
                context.Result = new GrantValidationResult(subject: userName, authenticationMethod: "custom", claims: claims);
                
            }
            catch(Exception ex)
            {
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
            return Task.CompletedTask;
        }
    }
}
