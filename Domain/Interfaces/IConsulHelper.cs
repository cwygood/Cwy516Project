using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IConsulHelper
    {
        Task<string> GetServiceUrl(string serviceName);
        void GetService();
    }
}
