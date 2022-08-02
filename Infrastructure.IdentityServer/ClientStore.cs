using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityServer
{
    public class ClientStore : IClientStore
    {
        private readonly IOptionsMonitor<IdentityServerConfiguration> _options;
        public ClientStore(IOptionsMonitor<IdentityServerConfiguration> options)
        {
            this._options = options;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            if (this._options.CurrentValue.Clients.Any(f => f.ClientId.Equals( clientId,StringComparison.OrdinalIgnoreCase)))
            {
                return Task.FromResult(this._options.CurrentValue.Clients.FirstOrDefault(f => f.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase)));
            }
            //数据库查找对应的client
            return null;
        }
    }
}
