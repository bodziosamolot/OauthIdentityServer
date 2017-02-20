using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;

namespace IdServer.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientId = "angular_client_implicit",
                    ClientName = "Trip Gallery (Implicit)",
                    Flow = Flows.Implicit,
                    AllowAccessToAllScopes = true,
                    AccessTokenType = AccessTokenType.Jwt,

                    // redirect = URI of the Angular application callback page
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44394/callback.html"
                    }
                },
                new Client
                {
                    ClientId = "mvc_client_client_credential",
                    ClientName = "MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("mvc_client_client_credential".Sha256())
                    },
                    Flow = Flows.ClientCredentials,
                    AllowAccessToAllScopes = true,
                    AccessTokenType = AccessTokenType.Jwt,

                    // redirect = URI of the Angular application callback page
                    RedirectUris = new List<string>
                    {
                        "http://localhost:56538/callback"
                    }
                },
                new Client
                {
                    ClientId = "mvc_client_auth_code",
                    ClientName = "MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("mvc_client_auth_code".Sha256())
                    },
                    Flow = Flows.AuthorizationCode,
                    AllowAccessToAllScopes = true,
                    AccessTokenType = AccessTokenType.Jwt,

                    // redirect = URI of the Angular application callback page
                    RedirectUris = new List<string>
                    {
                        "http://localhost:56539/callback"
                    }
                }
            };
        }
    }
}