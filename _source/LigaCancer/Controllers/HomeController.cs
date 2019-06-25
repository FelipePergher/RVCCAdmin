using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
