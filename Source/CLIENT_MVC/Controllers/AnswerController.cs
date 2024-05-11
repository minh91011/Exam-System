using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROJECT_PRN231.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace CLIENT_MVC.Controllers
{
    public class AnswerController : Controller
    {
        private readonly HttpClient client = null;
        private string BaseUrl = "";

        public AnswerController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:8080/api/Answer";
        }

        public async Task<IActionResult> List(int id)
        {
            HttpResponseMessage response = await client.GetAsync(BaseUrl + $"/GetByQuestionId/id?id={id}");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var list = System.Text.Json.JsonSerializer.Deserialize<List<Answer>>(strData, options) ?? new List<Answer>();
                ViewBag.QuestionId = id; // Truyền QuestionId qua ViewBag
                if (list.Any())
                {
                    ViewBag.Answers = list;
                    return View();
                }
                else
                {
                    ViewBag.QuestionId = id; // Truyền QuestionId qua ViewBag
                    return View();
                }
            }
            else
            {
                ViewBag.QuestionId = id; // Truyền QuestionId qua ViewBag
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> List(Answer answer)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(answer);
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

        public async Task<IActionResult> Index(int id)
        {
            HttpResponseMessage response = await client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var list = System.Text.Json.JsonSerializer.Deserialize<List<Answer>>(strData, options) ?? new List<Answer>();
                ViewBag.QuestionId = id; // Truyền QuestionId qua ViewBag
                if (list.Any())
                {
                    ViewBag.Answers = list;
                    return View();
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        //Add to list All
        [HttpGet]
        public async Task<IActionResult> AddToAll(int id)
        {
            HttpResponseMessage respone = await client.GetAsync("https://localhost:8080/api/Question");
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var list = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(strData, options) ?? new List<Question>();
            ViewBag.Questions = list;
            ViewBag.QuestionId = id;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(Answer answer)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(answer);
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
        public async Task<IActionResult> Update(int id)
        {
            using (var client = new HttpClient())
            {
                // Gửi yêu cầu API để lấy thông tin câu hỏi theo ID
                var response = await client.GetAsync(BaseUrl + $"/id?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu trả về từ API
                    var answerJson = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON thành đối tượng Question
                    var answer = JsonConvert.DeserializeObject<Answer>(answerJson);

                    return View(answer);
                }
                else
                {
                    // Xử lý khi không thành công
                    return View("Error");
                }
            }

        }


        [HttpPost]
        public async Task<IActionResult> Update(Answer answer)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(answer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Sử dụng đường dẫn API cho việc cập nhật câu hỏi
                HttpResponseMessage response = await client.PutAsync(BaseUrl + "/Update/" + answer.AnswerId, content);

                if (response.IsSuccessStatusCode)
                {
                    // Lấy URL của trang hiện tại và redirect đến nó
                    string currentUrl = Request.Headers["Referer"].ToString();
                    return Redirect(currentUrl);
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
        public async Task<IActionResult> Delete(Answer answer)
        {
            try
            {
                // Sử dụng đường dẫn API cho việc cập nhật câu hỏi
                HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "/Delete/" + answer.AnswerId);

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
