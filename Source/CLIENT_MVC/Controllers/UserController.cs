using CLIENT_MVC.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.Account;
using PROJECT_PRN231.Models.Mail;
using System.Net;
using PROJECT_PRN231.Utilities;

namespace CLIENT_MVC.Controllers
{
    public class UserController : Controller
    {
        const string CONTROLLER_USER = "User/";
        private readonly APIHelper _apiHelper;
        public UserController(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = await _apiHelper.RequestGetAsync<User>(CONTROLLER_USER + $"Detail/{username}", true);
            return View(user);
        }

        public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(ChangePassword model)
        {
            var result = await _apiHelper.RequestPutAsync<ChangePassword>(CONTROLLER_USER + "ChangePassword", model, true);
            if (result.Response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else if (result.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Validate = result.Body;
            }
            else if (result.Response.StatusCode == HttpStatusCode.Forbidden || result.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AuthorizeError", "Home");
            }
            return View(model);
        }

        public IActionResult AddEmail(string username)
        {
            var sendForm = new SendOTP
            {
                Username = username,
                Email = "sample@email.com"
            };
            return View(sendForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmail(SendOTP model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _apiHelper.RequestPostAsync<SendOTP>(CONTROLLER_USER + "SendMailOTP", model, true);
            if (result.Response.IsSuccessStatusCode)
            {
                var otpResponse = JsonConvert.DeserializeObject<OTPResponse>(result.Body);
                return RedirectToAction("InputOTP", new { Username = otpResponse.Username, Email = otpResponse.Email});
            }
            else if (result.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Validate = result.Body;
            }
            else if (result.Response.StatusCode == HttpStatusCode.Forbidden || result.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AuthorizeError", "Home");
            }
            return View(model);
        }

        public IActionResult InputOTP(string username, string email)
        {
            var otpResponse = new OTPResponse { Username = username, Email = email };
            return View(otpResponse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InputOTP(OTPResponse model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _apiHelper.RequestPostAsync<OTPResponse>(CONTROLLER_USER + "ConfirmOTP", model, true);
            if (result.Response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else if (result.Response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Validate = result.Body;
            }
            else if (result.Response.StatusCode == HttpStatusCode.Forbidden || result.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AuthorizeError", "Home");
            }
            return View(model);
        }


    }
}
