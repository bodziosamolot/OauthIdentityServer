// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using AuthServer;
using Microsoft.Extensions.Logging;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer4.Quickstart.UI
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly AccountService _account;
        private readonly ILogger _logger;
        private readonly IClientStore _clientStore;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            ILoggerFactory loggerFactory,
            IClientStore clientStore,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _account = new AccountService(interaction, httpContextAccessor, clientStore);
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var vm = await _account.BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            var returnUrl = model.ReturnUrl;

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(await BuildLoginViewModelAsync(model));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(await BuildLoginViewModelAsync(model));
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await _account.BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // no need to show prompt
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);
            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                try
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.Authentication.SignOutAsync(vm.ExternalAuthenticationScheme, 
                        new AuthenticationProperties { RedirectUri = url });
                }
                catch(NotSupportedException) // this is for the external providers that don't have signout
                {
                }
                catch(InvalidOperationException) // this is for Windows/Negotiate
                {
                }
            }

            // delete local authentication cookie
            await HttpContext.Authentication.SignOutAsync();

            return View("LoggedOut", vm);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var providers = HttpContext.Authentication.GetAuthenticationSchemes()
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.AuthenticationScheme
                });

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
                    }
                }
            }

            return new LoginViewModel
            {
                EnableLocalLogin = allowLocal,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

            ///// <summary>
            ///// Post processing of external authentication
            ///// </summary>
            //[HttpGet]
            //public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
            //{
            //    // read external identity from the temporary cookie
            //    var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            //    var tempUser = info?.Principal;
            //    if (tempUser == null)
            //    {
            //        throw new Exception("External authentication error");
            //    }

            //    // retrieve claims of the external user
            //    var claims = tempUser.Claims.ToList();

            //    // try to determine the unique id of the external user - the most common claim type for that are the sub claim and the NameIdentifier
            //    // depending on the external provider, some other claim type might be used
            //    var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            //    if (userIdClaim == null)
            //    {
            //        userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            //    }
            //    if (userIdClaim == null)
            //    {
            //        throw new Exception("Unknown userid");
            //    }

            //    // remove the user id claim from the claims collection and move to the userId property
            //    // also set the name of the external authentication provider
            //    claims.Remove(userIdClaim);
            //    var provider = info.Properties.Items["scheme"];
            //    var userId = userIdClaim.Value;

            //    // check if the external user is already provisioned
            //    var user = _users.FindByExternalProvider(provider, userId);
            //    if (user == null)
            //    {
            //        // this sample simply auto-provisions new external user
            //        // another common approach is to start a registrations workflow first
            //        user = _users.AutoProvisionUser(provider, userId, claims);
            //    }

            //    var additionalClaims = new List<Claim>();

            //    // if the external system sent a session id claim, copy it over
            //    var sid = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            //    if (sid != null)
            //    {
            //        additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            //    }

            //    // if the external provider issued an id_token, we'll keep it for signout
            //    AuthenticationProperties props = null;
            //    var id_token = info.Properties.GetTokenValue("id_token");
            //    if (id_token != null)
            //    {
            //        props = new AuthenticationProperties();
            //        props.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            //    }

            //    // issue authentication cookie for user
            //    await HttpContext.Authentication.SignInAsync(user.SubjectId, user.Username,  provider, props, additionalClaims.ToArray());

            //    // delete temporary cookie used during external authentication
            //    await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            //    // validate return URL and redirect back to authorization endpoint
            //    if (_interaction.IsValidReturnUrl(returnUrl))
            //    {
            //        return Redirect(returnUrl);
            //    }

            //    return Redirect("~/");
            //}
        }
}