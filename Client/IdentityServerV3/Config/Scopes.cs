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
                new Scope
                {
                    Name = "management",
                    DisplayName = "Management",
                    Description = "Management",
                    Type = ScopeType.Resource
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