using CLIENT_MVC.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;
using System.Net;

namespace CLIENT_MVC.Controllers
{
	public class TakingExamController : Controller
	{
		const string CONTROLLER_EXAMQUESTION = "ExamQuestion/";
		const string CONTROLLER_USEREXAMQUESTIONANSWER = "UserExamQuestionAnswer/";
		const string CONTROLLER_USEREXAMRESULT = "UserExamResult/";
		const string CONTROLLER_EXAM = "Exam/";

		private UserExamResult UserExamResult { get; set; }
		private readonly APIHelper _apiHelper;
		public TakingExamController(APIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}
		public async Task<IActionResult> Index(int? examId)
		{
			if (examId == null)
			{
				return BadRequest("No examId");
			}
			var getExam = await _apiHelper.RequestGetAsync<Exam>(CONTROLLER_EXAM + $"id?id={examId}");
			if (getExam != null) 
			{
				ViewData["ExamTime"] = getExam.Duration * 60;
			}
			var viewModel = new UserExamResultVM
			{
				ExamId = examId,
				Score = 0,
				StartTime = DateTime.Now,
				UserId = HttpContext.Session.GetInt32("UserId"),
				EndTime = null
			};
			var createResult = await _apiHelper.RequestPostAsync<UserExamResultVM>(CONTROLLER_USEREXAMRESULT, viewModel, true);
			if (createResult.Response.IsSuccessStatusCode)
			{
				var result = JsonConvert.DeserializeObject<UserExamResult>(createResult.Body);
				UserExamResult = result;
			}
			else
			{

				return Problem("Problem when adding result");
			}
			var questions = await _apiHelper.RequestGetAsync<List<ExamQuestion>>(CONTROLLER_EXAMQUESTION + $"examId/{examId.Value}");
			return View(questions);
		}
		[HttpPost]
		public async void SelectAnswer(string userId, string examId, string questionId, string answerId, string isCorrect)
		{
			var model = new UserExamQuestionAnswerVM
			{
				AnswerId = int.Parse(answerId),
				ExamId = int.Parse(examId),
				IsCorrect = bool.Parse(isCorrect),
				QuestionId = int.Parse(questionId),
				UserId = int.Parse(userId)
			};
			await _apiHelper.RequestPostAsync<UserExamQuestionAnswerVM>(CONTROLLER_USEREXAMQUESTIONANSWER + "SelectAnswer", model);
			//if (result.Response.IsSuccessStatusCode)
			//{
			//	ViewBag.Result = result.Body;
			//	//return RedirectToAction("Index", "Home");
			//}
			//else if (result.Response.StatusCode == HttpStatusCode.BadRequest || result.Response.StatusCode == HttpStatusCode.NotFound)
			//{
			//	ViewBag.Validate = result.Body;
			//}
		}

		public async Task<IActionResult> FinishExam(int? userId, int? examId)
		{
			var viewModel = new UserExamResultVM
			{
				ExamId = examId,
				UserId = userId,
			};
			var result = await _apiHelper.RequestPutAsync<UserExamResultVM>(CONTROLLER_USEREXAMRESULT + $"{viewModel.UserId}/{viewModel.ExamId}", viewModel, true);
			if (result.Response.IsSuccessStatusCode)
			{
				var model = JsonConvert.DeserializeObject<UserExamResult>(result.Body);
				return View(model);
			}
			else
			{
				return Problem(result.Body);
			}
		}
	}
}
