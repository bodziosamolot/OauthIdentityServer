using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Tracing;
using API.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Security.Jwt;

[assembly: OwinStartup(typeof(API.Startup))]

namespace API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            SystemDiagnosticsTraceWriter traceWriter = httpConfig.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Debug;

            ConfigureSwashbuckle(httpConfig);
            ConfigureWebApi(httpConfig);

            var certificate = new X509Certificate2(Convert.FromBase64String("MIIDHDCCAgSgAwIBAgIQ0GNlbF8ROJBJTjjJCykLrDANBgkqhkiG9w0BAQsFADAYMRYwFAYDVQQDEw1Cb2R6aW9TYW1vbG90MB4XDTE0MTIzMTIzMDAwMFoXDTE5MTIzMTIzMDAwMFowGDEWMBQGA1UEAxMNQm9kemlvU2Ftb2xvdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALG2o7JQ7z7iI7ykgyW6HAVqKR15ezdl0/qABRQAtQGio8OtcOWC/LZEmDEImUe3LrBvphDsTxVCAxXhN5YruAxcYo5gYZr4sD+fknkaUUlYIyXi3+N3q1zwr4ZY6BVN/0mnFXGaOxHOcYiimn1hdxWXT5kZT7ara8zIwxvCBT4uyLBVFeOebFipPcC1Uhnd+Q0/ogJ2gV1HKdPviTw81rHwRRCaDoP68Tks258AUcezxk29lY/QW1dn1MJZNrMj58w16diHV0hTCx2bOQsVmOZOWH/RLISYJzBwLInYcR8xwT7ftAVBWYdAx/9K5Mv9cOVadqqPUOjdE0boiua+YvcCAwEAAaNiMGAwEwYDVR0lBAwwCgYIKwYBBQUHAwMwSQYDVR0BBEIwQIAQTbHDFQrYgasQzWgHdeUT8qEaMBgxFjAUBgNVBAMTDUJvZHppb1NhbW9sb3SCENBjZWxfETiQSU44yQspC6wwDQYJKoZIhvcNAQELBQADggEBAFEV9WZcYKnyLtifoyU8/MV1k86o1SDH8paj0eViIoXG4ZeP3s1LQO9Xi4vAI4Dyvi/WZ8RTBcN+8AU5kUfsY+SWUOpBp8GjQ0+xo1rGBlrQIJI9IcUVwLD61YkREZqjA7lpyNBDZqrGUiNcEIVy3vapnMglsvBooTOPBIeiEqxzm79Bms/N34HzngGQyaOf8v0CTZPDJtadkSTkaJyFSxlF3RDIxYeP7yLZtIutFxOdRwm92/kreTZ0IhgUeGEtZIsgoSWEaSPxFZbBgcuv3E9pfARxggn4fy9vbCZxXMeqNSqEMbbEABuIBXTdfhoOBngXIjhFgp1iuGn5JW4mkGc="));

            //app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions()
            //{
            //    AllowedAudiences = new[] { "https://localhost:44308/identity/resources" },
            //    TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidAudience = "https://localhost:44308/identity/resources",
            //        ValidIssuer = "https://localhost:44308/identity",
            //        // Certificate has to be set in order for token validation to work.
            //        // Remove the cert and see the trace log.
            //        IssuerSigningKey = new X509SecurityKey(certificate)
            //    }
            //});

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = Constants.IdentityConstants.IssuerUri,
                RequiredScopes = new[] { "secret", "management" }
            });

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }

        private IContainer ConfigureAutoFac(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ValuesController>().InstancePerRequest();

            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }

        private void ConfigureSwashbuckle(HttpConfiguration config)
        {
            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
                .EnableSwaggerUi();
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
