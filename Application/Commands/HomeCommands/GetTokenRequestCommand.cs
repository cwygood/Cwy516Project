using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.HomeCommands
{
    public class GetTokenRequestCommand : IRequest<GetTokenResponseCommand>
    {
        public string Role { get; set; }
    }
}
