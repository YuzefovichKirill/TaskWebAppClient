using Microsoft.AspNetCore.Mvc;

namespace TaskWebAppClient.Controllers
{
    public class FridgeProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
