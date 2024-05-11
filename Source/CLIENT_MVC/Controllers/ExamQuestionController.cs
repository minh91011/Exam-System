using Microsoft.AspNetCore.Mvc;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CLIENT_MVC.Controllers
{
    public class ExamQuestionController : Controller
    {
        private readonly HttpClient client = null;
        private string BaseUrl = "";

        public ExamQuestionController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:8080/api/ExamQuestion";
        }


        public async Task<IActionResult> Index(int id)
        {
            HttpResponseMessage response = await client.GetAsync(BaseUrl + $"/examId/{id}");
            HttpResponseMessage responseQuestion = await client.GetAsync("https://localhost:8080/api/Question");
            if (response.IsSuccessStatusCode && responseQuestion.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                string strDataQuestion = await responseQuestion.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var list = System.Text.Json.JsonSerializer.Deserialize<List<ExamQuestion>>(strData, options) ?? new List<ExamQuestion>();
                var listQuestion = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(strDataQuestion, options) ?? new List<Question>();

                ViewBag.ExamId = id; // Truyền ExamId qua ViewBag
                ViewBag.ExamQuestions = list;
                ViewBag.Questions = listQuestion;
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(ExamQuestion examQuestion)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(examQuestion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/Add", content);

                if (response.IsSuccessStatusCode)
                {
                    // Lấy URL của trang hiện tại và redirect đến nó
                    string currentUrl = Request.Headers["Referer"].ToString();
                    return Redirect(currentUrl);
                }
                else
                {
                    Console.WriteLine(response);
                    // Xử lý lỗi nếu cần
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Xử lý exception nếu cần
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            HttpResponseMessage respone = await client.GetAsync("https://localhost:8080/api/Question");
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var list = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(strData, options) ?? new List<Question>();
            ViewBag.Questions = list;
            return View();
        }
    }
}
