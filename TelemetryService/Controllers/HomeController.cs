using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TelemetryService.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            var appInsights = TelemetryClientService.GetTelemetryClient();
            return await TelemetryClientService.LogDependency(async () => await GetDataAsync(), appInsights, "HTTPGET", "Google", "data");
        }

        private async Task<ActionResult> GetDataAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://www.google.com");
                var response = await httpClient.GetAsync(string.Empty);
                if (response.IsSuccessStatusCode)
                    return View();
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new WebException(error);
                }
            }
        }
    }
}