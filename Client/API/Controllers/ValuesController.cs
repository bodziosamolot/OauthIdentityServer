using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Helpers;

namespace API.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("ManagementValues")]
        [Authorize]
        public IHttpActionResult GetManagementValues()
        {
            if (!TokenHelper.IsClaimPresent("management"))
            {
                return Unauthorized();
            }

            return Ok(new string[] { "Management-value-1", "Management-value-2" });
        }

        [HttpGet]
        [Route("SecretValues")]
        [Authorize(Roles="SecretReader")]
        public IHttpActionResult GetSecretValues()
        {
            if (!TokenHelper.IsClaimPresent("secret"))
            {
                return Unauthorized();
            }

            return Ok(new string[] { "Secret-value-1", "Secret-value-2" });
        }

        [HttpGet]
        [Route("PublicValues")]
        public IHttpActionResult GetPublicValues()
        {
            return Ok(new string[] { "Public-value-1", "Public-value-2" });
        }
    }
}
