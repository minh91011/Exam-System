using CLIENT_MVC.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using PROJECT_PRN231;
using PROJECT_PRN231.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace CLIENT_MVC.Controllers
{
	public class LoginController : Controller
	{
		const string CONTROLLER_AUTH = "Auth/";
		const string CONTROLLER_USER = "User/";
		private readonly APIHelper _apiHelper;
        private readonly AppSettings _applicationSettings;
        public LoginController(APIHelper apiHelper, IOptions<AppSettings> applicationSettings)
		{
			_apiHelper = apiHelper;
			_applicationSettings = applicationSettings.Value;
		}
		public IActionResult Index()
		{
			ViewBag.Validate = "";
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(Login login)
		{
			if (ModelState.IsValid)
			{
				var result = await _apiHelper.RequestPostAsync<Login>(CONTROLLER_AUTH + "Login", login);
				if (result.Response.IsSuccessStatusCode)
				{
					var loginResult = JsonConvert.DeserializeObject<LoginResult>(result.Body);
                    try
                    {
                        // Decode the JWT token
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(loginResult.Token);

                        foreach(var claim in token.Claims)
						{
							if(claim.Type == "username") HttpContext.Session.SetString("Username", claim.Value);
                            if(claim.Type == "role") HttpContext.Session.SetString("Role", claim.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to decode JWT token: " + ex.Message);
                    }
                    _apiHelper.BearerToken = loginResult.Token;
                    HttpContext.Session.SetInt32("UserId", loginResult.Id);
                    return RedirectToAction("Index", "Home");
				}
				else if (result.Response.StatusCode == HttpStatusCode.BadRequest)
				{
					ViewBag.Validate = result.Body;
				}
				return View(login);
			}
			else
			{
				return View(login);
			}
		}

		public IActionResult ForgotPassword()
		{
			ViewBag.Result = "";
			ViewBag.Validate = "";
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ResetPassword resetPassword)
		{
			if (ModelState.IsValid)
			{
				var result = await _apiHelper.RequestPutAsync<ResetPassword>(CONTROLLER_USER + "ResetPassword", resetPassword);
				if (result.Response.IsSuccessStatusCode)
				{
					ViewBag.Result = result.Body;
					//return RedirectToAction("Index", "Home");
				}
				else if (result.Response.StatusCode == HttpStatusCode.BadRequest || result.Response.StatusCode == HttpStatusCode.NotFound)
				{
					ViewBag.Validate = result.Body;
				}
			}
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}


