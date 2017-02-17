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

                var options = new IdentityServerOptions
                {
                    Factory = idServerServiceFactory,
                    SiteName = "Security Token Service",
                    IssuerUri = IdentityConstants.IssuerUri,
                    PublicOrigin = IdentityConstants.Origin,
                    SigningCertificate = LoadCertificate(),
                    LoggingOptions = new LoggingOptions()
                    {
                        WebApiDiagnosticsIsVerbose = true,
                        EnableWebApiDiagnostics = true,
                        EnableKatanaLogging = true,
                        EnableHttpLogging = true
                    }
                };

                idsrvApp.UseIdentityServer(options);

                //var cert = LoadCertificate();
                //var privateKey = cert.PrivateKey.ToXmlString(true);
                //Console.WriteLine();
            });
        }

        private X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2($@"{AppDomain.CurrentDomain.BaseDirectory}Certificates\MyKey.pfx", "1qaz@WSX");
        }
    }
}