using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(Client.MVC.Startup))]

namespace Client.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier =
                IdentityModel.JwtClaimTypes.Name;

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "mvc_client_hybrid",
                Authority = Constants.IdentityConstants.IssuerUri,
                RedirectUri = Constants.IdentityConstants.MVCHybridCallback,
                SignInAsAuthenticationType = "Cookies",
                ResponseType = "code id_token token",
                Scope = "openid profile address management secret",

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthenticationFailed = async n =>
                    {
                        Console.WriteLine(n.Exception.Message);
                    },
                    #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                    SecurityTokenValidated = async n =>
                    {
                        Client.MVC.TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        Client.MVC.TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

                        var idTokenClaim = new Claim("id_token", n.ProtocolMessage.IdToken);

                        var givenNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

                        var scopeClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.Scope);

                        var familyNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.FamilyName);

                        var subClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.Subject);

                        // create a new claims, issuer + sub as unique identifier
                        var nameClaim = new Claim(IdentityModel.JwtClaimTypes.Name,
                                    Constants.IdentityConstants.IssuerUri + subClaim.Value);

                        var newClaimsIdentity = new ClaimsIdentity(
                           n.AuthenticationTicket.Identity.AuthenticationType,
                           IdentityModel.JwtClaimTypes.Name,
                           IdentityModel.JwtClaimTypes.Role);

                        if (nameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(nameClaim);
                        }

                        if (givenNameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(givenNameClaim);
                        }

                        if (familyNameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(familyNameClaim);
                        }

                        if (idTokenClaim != null)
                        {
                            newClaimsIdentity.AddClaim(idTokenClaim);
                        }

                        if (scopeClaim != null)
                        {
                            newClaimsIdentity.AddClaim(scopeClaim);
                        }

                        // add the access token so we can access it later on
                        newClaimsIdentity.AddClaim(
                            new Claim("access_token", n.ProtocolMessage.AccessToken));

                        // create a new authentication ticket, overwriting the old one.
                        n.AuthenticationTicket = new AuthenticationTicket(
                                                 newClaimsIdentity,
                                                 n.AuthenticationTicket.Properties);
                    },
                    #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
                    RedirectToIdentityProvider = n =>
                    {
                        // if signing out, add the id_token_hint
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");
                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
