using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaskWebAppClient.Models;
using TaskWebAppClient.Helper;
using System.Net.Http.Json;

namespace TaskWebAppClient.Controllers
{
    public class HomeController : Controller
    {
        private FridgeAPI _api = new FridgeAPI();

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Procedure()
        {
            HttpClient client = _api.Initial();

            await client.PostAsJsonAsync<FridgeProduct>("api/fridgeproduct", null);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
