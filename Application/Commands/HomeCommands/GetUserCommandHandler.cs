using Application.Mq;
using Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.HomeCommands
{
    public class GetUserCommandHandler : IRequestHandler<GetUserRequestCommand, GetUserResponseCommand>
    {
        private readonly IUserRepository _userRepository;
        public GetUserCommandHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public Task<GetUserResponseCommand> Handle(GetUserRequestCommand request, CancellationToken cancellationToken)
        {
            var userInfo = this._userRepository.GetUserById(request.Id);
            if (userInfo == null)
            {
                return Task.FromResult(new GetUserResponseCommand()
                {
                    RequestId = request.RequestId,
                    Code = "1",
                    IsSuccess = false,
                    Messages = new System.Collections.Generic.List<string>() { "查询失败！" }
                });
            }
            return Task.FromResult(new GetUserResponseCommand()
            {
                RequestId = request.RequestId,
                Code = "0",
                IsSuccess = true,
                Messages = new System.Collections.Generic.List<string>() { "查询成功！" },
                Data = userInfo
            });
        }
    }
}
