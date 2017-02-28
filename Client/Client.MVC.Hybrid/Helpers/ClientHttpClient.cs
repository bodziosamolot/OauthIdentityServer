using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Constants;
using IdentityModel.Client;

namespace Client.MVC
{
    public static class ClientHttpClient
    {

        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();

            var token = (HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst("access_token");
            if (token != null)
            {
                client.SetBearerToken(token.Value);
            }

            client.BaseAddress = new Uri(IdentityConstants.API);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

    }
}