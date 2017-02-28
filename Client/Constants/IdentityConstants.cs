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

        public const string MVCClientSecret = "mvc_client_client_credential";
        public const string MVCClientSecretAuthCode = "mvc_client_auth_code";
        public const string MVCClientSecretHybrid = "mvc_client_hybrid";

        public const string MVCCallback = "http://localhost:56538/callback";
        public const string MVCAuthCodeCallback = "http://localhost:56539/callback";
        public const string MVCHybridCallback = "http://localhost:56541/Values/Index";
    }
}
