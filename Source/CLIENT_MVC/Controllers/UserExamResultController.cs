using CLIENT_MVC.Utilities;
using Microsoft.AspNetCore.Mvc;
using PROJECT_PRN231.Models;

namespace CLIENT_MVC.Controllers
{
	public class UserExamResultController : Controller
	{
		const string CONTROLLER_USEREXAMRESULT = "UserExamResult/";
		private readonly APIHelper _apiHelper;
		public UserExamResultController(APIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task<IActionResult> Index()
		{
			var userId = HttpContext.Session.GetInt32("UserId");
			var result = await _apiHelper.RequestGetAsync<List<UserExamResult>>(CONTROLLER_USEREXAMRESULT + $"userId/{userId}", true);
			return View(result);
		}
	}
}
