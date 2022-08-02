using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands
{
    public interface IApiAction
    {
    }
    public class BaseCommand : IApiAction
    {
        public string RequestId { get; set; }
    }
}
