using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaskWebAppClient.Models;
using TaskWebAppClient.Helper;
using TaskWebAppClient.Models;
using System.Net.Http.Json;

namespace TaskWebAppClient.Controllers
{
    public class FridgesController : Controller
    {
        private FridgeAPI _api = new FridgeAPI();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Fridge> fridges = new List<Fridge>();
            HttpClient client = _api.Initial();

            HttpResponseMessage res = await client.GetAsync("api/fridge");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                fridges = JsonConvert.DeserializeObject<List<Fridge>>(result);
            }

            return View(fridges);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Fridge fridge)
        {
            if (!ModelState.IsValid)
            {
                return View(fridge);
            }

            HttpClient client = _api.Initial();

            var insertRecord = await client.PostAsJsonAsync<Fridge>("api/fridge", fridge);

            if (insertRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(fridge);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            HttpClient client = _api.Initial();
            Fridge dbFridge = null;

            var editRecord = await client.GetAsync("api/fridge/" + id.ToString());

            if (editRecord.IsSuccessStatusCode)
            {
                var result = editRecord.Content.ReadAsStringAsync().Result;
                dbFridge = JsonConvert.DeserializeObject<Fridge>(result);
            }

            return View(dbFridge);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,OwnerName,FridgeModelId")] Fridge fridge)
        {
            HttpClient client = _api.Initial();

            HttpResponseMessage response = await client.PutAsJsonAsync("api/fridge/" + id.ToString(), fridge);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.message = "Fridge record isn\'t updated";
            }

            return View("Edit", fridge);

        }


        public async Task<IActionResult> Delete(Guid id)
        {
            HttpClient client = _api.Initial();
            Fridge dbFridge = null;

            var res = await client.GetAsync("api/fridge/" + id.ToString());

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dbFridge = JsonConvert.DeserializeObject<Fridge>(result);
            }
            else
            {
                return View("Error");
            }

            return View(dbFridge);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            HttpClient client = _api.Initial();

            var deleteRecord = await client.DeleteAsync("api/fridge/" + id.ToString());

            if (deleteRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Error");
            }

        }
    }
}
