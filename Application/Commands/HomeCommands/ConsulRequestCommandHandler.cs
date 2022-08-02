using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.HomeCommands
{
    public class ConsulRequestCommandHandler : IRequestHandler<ConsulRequestCommand, BaseResponseCommand>
    {
        private readonly IConsulRepository _consulRepository;
        public ConsulRequestCommandHandler(IConsulRepository consulRepository)
        {
            this._consulRepository = consulRepository;
        }
        public async Task<BaseResponseCommand> Handle(ConsulRequestCommand request, CancellationToken cancellationToken)
        {
            var response = await this._consulRepository.SendRequest(request.ServiceName);
            return await Task.FromResult(new BaseResponseCommand()
            {
                Code = "1",
                Messages = new List<string>() { response }
            });
        }
    }
}
