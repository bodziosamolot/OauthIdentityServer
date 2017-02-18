using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class IdentityConstants
    {
        public const string IssuerUri = "https://localhost:44308/identity";
        public const string Origin = "https://localhost:44308";
        public const string API = "https://localhost:44396/api/values";
        public const string AuthEndoint = "https://localhost:44308/identity/connect/authorize";
        public const string TokenEndoint = "https://localhost:44308/identity/connect/token";

        public const string MVCClientSecret = "myrandomclientsecret";
        public const string MVCCallback = "http://localhost:56538/callback";
    }
}
