using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.HomeCommands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserRequestCommand, AddUserResponseCommand>
    {
        private readonly IUserRepository _userRepository;
        public AddUserCommandHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public Task<AddUserResponseCommand> Handle(AddUserRequestCommand request, CancellationToken cancellationToken)
        {
            var isAdd = this._userRepository.AddUser(new Domain.Models.User_Info()
            {
                Name = request.UserName,
                Code = request.UserCode,
                Password = request.Password
            });
            this._userRepository.AddMongoDb(new Domain.Models.User_Info()
            {
                Name = request.UserName,
                Code = request.UserCode,
                Password = request.Password
            });
            if (isAdd)
            {
                return Task.FromResult(ApiResult.OK<AddUserResponseCommand>("新增成功", request.RequestId));
            }
            return Task.FromResult(ApiResult.Failure<AddUserResponseCommand>("新增失败", request.RequestId));
        }
    }
}
