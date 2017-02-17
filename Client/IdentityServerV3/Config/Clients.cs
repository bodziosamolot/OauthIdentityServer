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


                    // redirect = URI of the Angular application callback page
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44394/callback.html"
                    }
                }
            };
        }
    }
}