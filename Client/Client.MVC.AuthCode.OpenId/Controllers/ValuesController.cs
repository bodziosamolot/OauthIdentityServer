using Client.MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.MVC.Controllers
{
    public class ValuesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Values";

            var httpClient = ClientHttpClient.GetClient();

            var publicValues = await httpClient.GetAsync("values/PublicValues").ConfigureAwait(false);
            var secretValues = await httpClient.GetAsync("values/SecretValues").ConfigureAwait(false);
            var managementValues = await httpClient.GetAsync("values/ManagementValues").ConfigureAwait(false);

            var valuesAsString = await publicValues.Content.ReadAsStringAsync().ConfigureAwait(false);
            var secretAsString = await secretValues.Content.ReadAsStringAsync().ConfigureAwait(false);
            var managementAsString = await managementValues.Content.ReadAsStringAsync().ConfigureAwait(false);

            var publicValuesDeserialized = publicValues.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<string>>(valuesAsString)
                : null;

            var secretValuesDeserialized = secretValues.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<string>>(secretAsString)
                : null;

            var managementValuesDeserialized = managementValues.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<string>>(managementAsString)
                : null;

            var vm = new ValueViewModel(publicValuesDeserialized,
                secretValuesDeserialized,
                managementValuesDeserialized);

            return View(vm);
        }
    }
}
