using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using IdentityModel.Client;

namespace Client.MVC.Controllers
{
    public class CallbackController : Controller
    {
        // GET: STSCallback
        public async Task<ActionResult> Index()
        {
            // get the authorization code from the query string
            var authCode = Request.QueryString["code"];

            // with the auth code, we can request an access token.
            var client = new TokenClient(
                IdentityConstants.TokenEndoint,
                "mvc_client_auth_code",
                 IdentityConstants.MVCClientSecretAuthCode);

            var tokenResponse = await client.RequestAuthorizationCodeAsync(
                authCode,
                IdentityConstants.MVCAuthCodeCallback);

            // we save the token in a cookie for use later on
            Response.Cookies["ClientMVCCookie.AuthCode"]["access_token"] = tokenResponse.AccessToken;

            // get the state (uri to return to)
            var state = Request.QueryString["state"];

            // redirect to the URI saved in state
            return Redirect(state);
        }
    }
}