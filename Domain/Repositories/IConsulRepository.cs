using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IConsulRepository : IBaseRepository
    {
        Task<string> SendRequest(string serviceName);
    }
}
