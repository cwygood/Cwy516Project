using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.Common.Mq;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.HomeCommands
{
    public class GetAllUserCommandHandler : IRequestHandler<GetAllUserRequestCommand, GetAllUserResponseCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisCache _cache;
        private readonly RabbitMqClient _mqClient;
        private readonly IHttpService _httpService;
        public GetAllUserCommandHandler(IUserRepository userRepository, IRedisCache cache, RabbitMqClient mqClient, IHttpService httpService)
        {
            this._userRepository = userRepository;
            this._cache = cache;
            this._mqClient = mqClient;
            this._httpService = httpService;
        }
        public async Task<GetAllUserResponseCommand> Handle(GetAllUserRequestCommand request, CancellationToken cancellationToken)
        {
            var userInfos = this._userRepository.GetAllUser();
            if (!userInfos.Any())
            {
                return await Task.FromResult(new GetAllUserResponseCommand() { Code = "1", IsSuccess = false, Messages = new List<string>() { "查询数据失败！" } });
            }
            var res = await this._httpService.GetAsync("http://10.81.3.167:5100/api/Home/PPP");
            this._mqClient.PushMessage("user.test", userInfos, "516project");//rabbitmq使用
            return await Task.FromResult(new GetAllUserResponseCommand()
            {
                RequestId = request.RequestId,
                Code = "0",
                IsSuccess = true,
                Messages = new List<string>() { "成功！" },
                Data = userInfos
            });
        }
    }
}
