using CLIENT_MVC.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN231.Models.Account;
using System.Net;

namespace CLIENT_MVC.Controllers
{
    public class RegisterController : Controller
    {
        const string ACCESS_CONTROLLER = "Auth/";
        private readonly APIHelper _apiHelper;
        public RegisterController(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            ViewBag.Validate = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiHelper.RequestPostAsync<Register>(ACCESS_CONTROLLER + "Register", register);
                if(result.Response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","Home");
                }
                else if (result.Response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.Validate = result.Body;
                    return View(register);
                }
            }
            return View(register);
        }
    }
}
