
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CLIENT_MVC.Controllers
{
    public class QuestionController : Controller
    {
        private readonly HttpClient client = null;
        private string BaseUrl = "";

        public QuestionController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:8080/api/Question";
        }

        public async Task<IActionResult> Index()
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

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Question question)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(question);
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
                    var questionJson = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON thành đối tượng Question
                    var question = JsonConvert.DeserializeObject<Question>(questionJson);

                    return View(question);
                }
                else
                {
                    // Xử lý khi không thành công
                    return View("Error");
                }
            }

        }


        [HttpPost]
        public async Task<IActionResult> Update(Question question)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(question);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Sử dụng đường dẫn API cho việc cập nhật câu hỏi
                HttpResponseMessage response = await client.PutAsync(BaseUrl + "/Update/" + question.QuestionId, content);

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



        [HttpDelete]
        public async Task<IActionResult> Delete(Question question)
        {
            try
            {
                // Sử dụng đường dẫn API cho việc cập nhật câu hỏi
                HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "/Delete/" + question.QuestionId);

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