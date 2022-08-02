using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.HomeCommands
{
    public class ConsulRequestCommand : BaseCommand, IRequest<BaseResponseCommand>
    {
        public string ServiceName { get; set; }
    }
}
