using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Domain.Interfaces
{
    public interface IJwtToken
    {
        string GetToken(Claim[] claims);
    }
}
