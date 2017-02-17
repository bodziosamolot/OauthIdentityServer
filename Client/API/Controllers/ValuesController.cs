using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("ManagementValues")]
        [Authorize]
        public IEnumerable<string> GetManagementValues()
        {
            return new string[] { "Management-value-1", "Management-value-2" };
        }

        [HttpGet]
        [Route("SecretValues")]
        [Authorize]
        public IEnumerable<string> GetSecretValues()
        {
            return new string[] { "Secret-value-1", "Secret-value-2" };
        }

        [HttpGet]
        [Route("PublicValues")]
        public IEnumerable<string> GetPublicValues()
        {
            return new string[] { "Public-value-1", "Public-value-2" };
        }
    }
}
