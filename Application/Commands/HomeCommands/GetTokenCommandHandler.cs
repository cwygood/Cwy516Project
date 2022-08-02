using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.HomeCommands
{
    public class GetTokenCommandHandler : IRequestHandler<GetTokenRequestCommand, GetTokenResponseCommand>
    {
        private readonly IJwtToken _jwtToken;
        public GetTokenCommandHandler(IJwtToken jwtToken)
        {
            this._jwtToken = jwtToken;
        }
        public Task<GetTokenResponseCommand> Handle(GetTokenRequestCommand request, CancellationToken cancellationToken)
        {
            List<Claim> claims = new List<Claim>();
            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                claims.Add(new Claim("Roles", request.Role));
            }
            var token = this._jwtToken.GetToken(claims.ToArray());
            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(new GetTokenResponseCommand()
                {
                    Code = "1",
                    IsSuccess = false,
                    Messages = new List<string>() { "获取token失败" }
                });
            }
            return Task.FromResult(new GetTokenResponseCommand()
            {
                Code = "0",
                IsSuccess = true,
                Data = token,
                Messages = new List<string>() { "获取token成功！" }
            });
        }
    }
}
