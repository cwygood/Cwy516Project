using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.HomeCommands
{
    public class GetUserRequestCommand : BaseCommand, IRequest<GetUserResponseCommand>
    {
        public long Id { get; set; }
    }
}
