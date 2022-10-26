using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using TaskWebAppClient.Models;
using TaskWebAppClient.Helper;
using TaskWebAppClient.Models;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using Microsoft.CodeAnalysis;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Security.Policy;

namespace TaskWebAppClient.Controllers
{
    public class ProductsController : Controller
    {
        private FridgeAPI _api = new FridgeAPI();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();
            HttpClient client = _api.Initial();

            var getRecords = await client.GetAsync("api/product");

            if (getRecords.IsSuccessStatusCode)
            {
                var result = getRecords.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<Product>>(result);
            }

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            HttpClient client = _api.Initial();

            var insertRecord = await client.PostAsJsonAsync<Product>("api/product", product);

            if (insertRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.message = insertRecord.Content.ReadAsStringAsync().Result;
                return View(product);
            }

        }

        public async Task<IActionResult> Edit(Guid id)
        {
            HttpClient client = _api.Initial();
            Product dbProduct = null;

            var getRecord = await client.GetAsync("api/product/" + id.ToString());

            if (getRecord.IsSuccessStatusCode)
            {
                var result = getRecord.Content.ReadAsStringAsync().Result;
                dbProduct = JsonConvert.DeserializeObject<Product>(result);
            }

            return View(dbProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DefaultQuantity,Name")] Product product)
        {
            HttpClient client = _api.Initial();

            var editRecord = await client.PutAsJsonAsync("api/product/" + id.ToString(), product);

            if (editRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.message = editRecord.Content.ReadAsStringAsync().Result;
                return View("Edit", product);
            }
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            HttpClient client = _api.Initial();
            Product dbProduct = new();

            var getRecord = await client.GetAsync("api/product/" + id.ToString());

            if (getRecord.IsSuccessStatusCode)
            {
                var result = getRecord.Content.ReadAsStringAsync().Result;
                dbProduct = JsonConvert.DeserializeObject<Product>(result);

            }

            return View(dbProduct);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            HttpClient client = _api.Initial();

            var deleteRecord = await client.DeleteAsync("api/product/" + id.ToString());

            if (deleteRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.message = deleteRecord.Content.ReadAsStringAsync().Result;
                return View(nameof(Delete), new Product());
            }
        }
    }
}
