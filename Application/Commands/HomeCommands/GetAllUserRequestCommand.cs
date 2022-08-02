using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace Application.Commands.HomeCommands
{
    public class GetAllUserRequestCommand : BaseCommand, IRequest<GetAllUserResponseCommand>
    {
        
    }
}
