using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;

namespace IdServer.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.AddressAlwaysInclude,
                new Scope
                {
                    Name = "management",
                    DisplayName = "Management",
                    Description = "Management",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim("role", false)
                    }
                },
                new Scope
                {
                    Name = "secret",
                    DisplayName = "Secret",
                    Description = "Secret",
                    Type = ScopeType.Resource
                }
            };
        }
    }
}