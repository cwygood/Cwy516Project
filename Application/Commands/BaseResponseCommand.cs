using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands
{
    public class BaseResponseCommand : BaseCommand
    {
        public object Data { get; set; }
        public string Code { get; set; }
        public List<string> Messages { get; set; }
        public bool IsSuccess { get; set; }
    }
}
