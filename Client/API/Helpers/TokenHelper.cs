using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace API.Helpers
{
    public class TokenHelper
    {
        public static bool IsClaimPresent(string claimName)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;

            if (identity == null)
                throw new HttpException("No ClaimsIdentity present");

            return identity.Claims.FirstOrDefault(claim => claim.Value == claimName)!=null;
        }
    }
}