using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROJECT_PRN231.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace CLIENT_MVC.Controllers
{
    public class ExamController : Controller
    {
        private readonly HttpClient client = null;
        private string BaseUrl = "";

        public ExamController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:8080/api/Exam";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage respone = await client.GetAsync("https://localhost:8080/api/Exam");
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var list = System.Text.Json.JsonSerializer.Deserialize<List<Exam>>(strData, options) ?? new List<Exam>();
            ViewBag.Exams = list;
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListAllQuestion(int? id)
        {
            HttpResponseMessage respone = await
                client.GetAsync("https://localhost:8080/api/Question/GetAllByExamId?id=" + id);
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var list = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(strData, options) ?? new List<Question>();
            ViewBag.Questions = list;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestionToExamPost(int QuestionId, int ExamId)
        {
            ExamQuestion examQuestion = new ExamQuestion();
            examQuestion.QuestionId = QuestionId;
            examQuestion.ExamId = ExamId;
            var json = System.Text.Json.JsonSerializer.Serialize(examQuestion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://localhost:8080/api/ExamQuestion", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine(response);
                // Xử lý lỗi nếu cần
                return View("Error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(Exam exam)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(exam);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/Add", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
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
        public async Task<IActionResult> Update(int id)
        {
            using (var client = new HttpClient())
            {
                // Gửi yêu cầu API để lấy thông tin câu hỏi theo ID
                var response = await client.GetAsync(BaseUrl + $"/id?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu trả về từ API
                    var examJson = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON thành đối tượng Question
                    var exam = JsonConvert.DeserializeObject<Exam>(examJson);

                    return View(exam);
                }
                else
                {
                    // Xử lý khi không thành công
                    return View("Error");
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(Exam exam)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(exam);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Sử dụng đường dẫn API cho việc cập nhật câu hỏi
                //HttpResponseMessage response = await client.PutAsync(BaseUrl + "/Update/" + exam.ExamId, content);
                HttpResponseMessage response = await client.PutAsync("https://localhost:8080/api/Exam/Update/" + exam.ExamId, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
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



        [HttpPost]
        public async Task<IActionResult> Delete(Exam exam)
        {
            try
            {
                // Sử dụng đường dẫn API cho việc cập nhật câu hỏi
                HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "/Delete/" + exam.ExamId);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
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
    }
}
