using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskWebAppClient.Models;
using TaskWebAppClient.Helper;
using TaskWebAppClient.Models;

namespace TaskWebAppClient.Controllers
{
    public class FridgeProductsController : Controller
    {
        private FridgeAPI _api = new FridgeAPI();

        [HttpGet]
        public async Task<IActionResult> Index(Guid fridgeId)
        {
            List<Product> products = new List<Product>();
            Fridge fridge = null;
            HttpClient client = _api.Initial();

            var getRecord = await client.GetAsync("api/fridge/" + fridgeId.ToString());

            if (getRecord.IsSuccessStatusCode)
            {
                var result = getRecord.Content.ReadAsStringAsync().Result;
                fridge = JsonConvert.DeserializeObject<Fridge>(result);
            }

            var getRecords = await client.GetAsync("api/fridgeproduct/" + fridgeId.ToString());
            if (getRecords.IsSuccessStatusCode)
            {
                var result = getRecords.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<Product>>(result);
            }

            FridgeProductVM fridgeProductsVM = new FridgeProductVM() { fridge = fridge, products = products };
            return View(fridgeProductsVM);
        }

        public async Task<IActionResult> Add(Guid fridgeId)
        {
            Fridge fridge = null;
            HttpClient client = _api.Initial();

            FridgeProduct fridgeProduct = new FridgeProduct() { FridgeId = fridgeId };

            return View(fridgeProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid fridgeId,
            [Bind("FridgeId,ProductId,Quantity")] FridgeProduct fridgeProduct)
        {

            if (!ModelState.IsValid)
            {
                return View(fridgeProduct);
            }

            HttpClient client = _api.Initial();

            var insertRecord = await client.PostAsJsonAsync<FridgeProduct>("api/fridgeproduct/" + fridgeProduct.FridgeId.ToString()
                                                                           + "/" + fridgeProduct.ProductId.ToString(),fridgeProduct);

            if (insertRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index), new { fridgeId = fridgeId });
            }
            else
            {
                ViewBag.message = insertRecord.Content.ReadAsStringAsync().Result;
                return View(fridgeProduct);
            }
        }

        public async Task<IActionResult> Edit(Guid fridgeId, Guid productId)
        {
            HttpClient client = _api.Initial();
            FridgeProduct fridgeProduct = new();
            Product product = new();

            HttpResponseMessage response = await client.GetAsync("api/fridgeproduct/" + fridgeId.ToString() + "/" + productId.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(result);

                fridgeProduct = new() { FridgeId = fridgeId, ProductId = productId, Quantity = product.DefaultQuantity };
            }

            return View(fridgeProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid fridgeId, Guid productId,
            [Bind("FridgeId,ProductId,Quantity")] FridgeProduct fridgeProduct)
        {
            HttpClient client = _api.Initial();

            var editRecord = await client.PutAsJsonAsync("api/fridgeproduct/" + fridgeId.ToString() + "/" + productId.ToString(), fridgeProduct);

            if (editRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index), new { fridgeId = fridgeId });
            }
            else
            {
                ViewBag.message = editRecord.Content.ReadAsStringAsync().Result;
                return View(fridgeProduct);
            }
        }

        public async Task<IActionResult> Delete(Guid fridgeId, Guid productId)
        {
            HttpClient client = _api.Initial();
            Product product = new();
            FridgeProduct fridgeProduct = new();

            var getRecord = await client.GetAsync("api/fridgeproduct/" + fridgeId.ToString() + "/" + productId.ToString());
            if (getRecord.IsSuccessStatusCode)
            {
                var result = getRecord.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(result);

                fridgeProduct = new FridgeProduct()
                    { FridgeId = fridgeId, ProductId = productId, Quantity = product.DefaultQuantity };
            }

            return View(fridgeProduct);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid FridgeId, Guid productId)
        {
            HttpClient client = _api.Initial();

            var deleteRecord = await client.DeleteAsync("api/fridgeproduct/" + FridgeId.ToString() + "/" + productId.ToString());

            if (deleteRecord.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index), new { fridgeId = FridgeId });
            }
            else
            {
                ViewBag.message = deleteRecord.Content.ReadAsStringAsync().Result;
                return View(nameof(Delete), new FridgeProduct());
            }

        }
    }
}
