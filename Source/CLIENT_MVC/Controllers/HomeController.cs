using Microsoft.AspNetCore.Mvc;

namespace CLIENT_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AuthorizeError()
        {
            return View();
        }
    }
}
