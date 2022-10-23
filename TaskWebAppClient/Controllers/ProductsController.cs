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
using TaskWebAPIServer.Models;
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

            HttpResponseMessage res = await client.GetAsync("api/product");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
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

            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            HttpClient client = _api.Initial();
            Product dbProduct = null;

            var res = await client.GetAsync("api/product/" + id.ToString());

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dbProduct = JsonConvert.DeserializeObject<Product>(result);
            }

            return View(dbProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DefaultQuantity,Name")] Product product)
        {
            HttpClient client = _api.Initial();

            /*var patchDocument = new JsonPatchDocument<Product>();
            patchDocument.Replace(p => p, product);
            var serializedDocument = JsonConvert.SerializeObject(patchDocument);
            var requestContent = new StringContent(serializedDocument, Encoding.UTF8, "application/json-patch+json");*/


             HttpResponseMessage response = await client.PutAsJsonAsync("api/product/" + id.ToString(), product);

             if (response.IsSuccessStatusCode)
             {
                 return RedirectToAction(nameof(Index));
             }
             else
             {
                 ViewBag.message = "Product record isn\'t updated";
             }

             return View("Edit", product);


            /*var request = new HttpRequestMessage(new HttpMethod("PATCH"), "api/product/" + id.ToString())
             {
                 Content = requestContent
             };

            var editRecord = await client.SendAsync(request);

            if (editRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.message = "Product record isn\'t updated";
            }

            return View("Edit", product);*/
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            HttpClient client = _api.Initial();
            Product dbProduct = null;

            var res = await client.GetAsync("api/product/" + id.ToString());

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                dbProduct = JsonConvert.DeserializeObject<Product>(result);

            }

            return View(dbProduct);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            HttpClient client = _api.Initial();

            var deleteRecord = await client.DeleteAsync("api/product/" + id.ToString());

            return RedirectToAction(nameof(Index));
        }
    }
}
