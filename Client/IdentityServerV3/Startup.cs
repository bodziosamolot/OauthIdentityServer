using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using IdentityServer3.Core.Configuration;
using Owin;
using Constants;
using IdServer.Config;
using IdentityServer3.Core.Logging;
using IdentityServer3.Core.Services.Default;
using Serilog;

namespace IdServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map(string.Empty, idsrvApp =>
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Trace()
                    .CreateLogger();

                var idServerServiceFactory = new IdentityServerServiceFactory()
                                .UseInMemoryClients(Clients.Get())
                                .UseInMemoryScopes(Scopes.Get())
                                .UseInMemoryUsers(Users.Get());

                var corsPolicyService = new DefaultCorsPolicyService()
                {
                    AllowAll = true
                };

                idServerServiceFactory.CorsPolicyService = new
                    Registration<IdentityServer3.Core.Services.ICorsPolicyService>(corsPolicyService);

                var options = new IdentityServerOptions
                {
                    Factory = idServerServiceFactory,
                    SiteName = "Security Token Service",
                    IssuerUri = IdentityConstants.IssuerUri,
                    PublicOrigin = IdentityConstants.Origin,
                    SigningCertificate = LoadCertificate(),
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        EnablePostSignOutAutoRedirect = true,
                        PostSignOutAutoRedirectDelay = 5
                    },
                    LoggingOptions = new LoggingOptions()
                    {
                        WebApiDiagnosticsIsVerbose = true,
                        EnableWebApiDiagnostics = true,
                        EnableKatanaLogging = true,
                        EnableHttpLogging = true
                    }
                };

                idsrvApp.UseIdentityServer(options);
            });
        }

        private X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2($@"{AppDomain.CurrentDomain.BaseDirectory}Certificates\MyKey.pfx", "1qaz@WSX");
        }
    }
}