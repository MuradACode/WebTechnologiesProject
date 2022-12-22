using Microsoft.AspNetCore.Mvc;

namespace BrandShopMVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
