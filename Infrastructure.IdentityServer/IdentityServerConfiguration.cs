using IdentityServer4.Models;
using IdentityServer4.Test;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.IdentityServer
{
    public class IdentityServerConfiguration
    {
        public IdentityServerConfiguration()
        {
            //this.ApiScopes = new ApiScope[]
            //{
            //    new ApiScope("scope1","display scope1",new string[]{ })
            //};
            //this.ApiResources = new ApiResource[]
            //{
            //    new ApiResource("api1","display api1",new string[]{ })
            //};
            //this.Clients = new Client[]
            //{
            //    new Client()
            //    {
            //        ClientId="client",
            //        AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials,
            //        ClientSecrets =
            //        {
            //            new Secret("secret".Sha256())
            //        },
            //        AllowedScopes =
            //        {
            //            "scope1"
            //        }
            //    }
            //};
            //this.Users = new TestUser[]
            //{
            //    new TestUser()
            //    {
            //        SubjectId="1",
            //        Username="cwy",
            //        Password="123456"
            //    }
            //};
        }
        private List<ApiScope> _apiScopes;
        public IEnumerable<ApiScope> ApiScopes
        {
            get
            {
                return this._apiScopes;
            }
            set
            {
                this._apiScopes = new List<ApiScope>();
                foreach(var apiScope in value)
                {
                    this._apiScopes.Add(new ApiScope(apiScope.Name, apiScope.DisplayName, apiScope.UserClaims));
                }
            }
        }
        private List<ApiResource> _apiResources;
        public IEnumerable<ApiResource> ApiResources
        {
            get
            {
                return this._apiResources;
            }
            set
            {
                this._apiResources = new List<ApiResource>();
                foreach(var apiResource in value)
                {
                    this._apiResources.Add(new ApiResource(apiResource.Name, apiResource.DisplayName, apiResource.UserClaims));
                }
            }
        }
        private List<Client> _clients;
        public IEnumerable<Client> Clients
        {
            get
            {
                return this._clients;
            }
            set
            {
                this._clients = new List<Client>();
                foreach(var client in value)
                {
                    var secrets = new List<Secret>();
                    foreach(var clientSecret in client.ClientSecrets)
                    {
                        secrets.Add(new Secret(clientSecret.Value.Sha256()));
                    }
                    this._clients.Add(new Client()
                    {
                        ClientId = client.ClientId,
                        AllowedGrantTypes = client.AllowedGrantTypes,
                        ClientSecrets = secrets,
                        AllowedScopes = client.AllowedScopes
                    });
                }
            }
        }
        private List<TestUser> _users;
        public IEnumerable<TestUser> Users
        {
            get
            {
                return this._users;
            }
            set
            {
                this._users = new List<TestUser>();
                foreach(var user in value)
                {
                    this._users.Add(new TestUser()
                    {
                        SubjectId = user.SubjectId,
                        Username = user.Username,
                        Password = user.Password
                    });
                }
            }
        }
    }
}
