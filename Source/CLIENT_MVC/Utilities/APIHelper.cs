using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PROJECT_PRN231.Models.API;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace CLIENT_MVC.Utilities
{
    public class APIHelper
    {
        const string BaseUrl = "https://localhost:8080/api/";
        public string BearerToken { get; set; } = string.Empty;

        /// <summary>
        /// Call get request.
        /// </summary>
        /// <param name="path">path to api route, it will be append with baseUrl.</param>
        /// <param name="useAuthorization">Use Authorization.</param>
        /// <returns>return a generic type of the get request.</returns>
        public async Task<T> RequestGetAsync<T>(string path, bool useAuthorization = false)
        {
            using (var client = new HttpClient())
            {
                if (useAuthorization)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
                }
                var response = await client.GetAsync(BaseUrl + path);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<T>(responseBody);
                    return responseData;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<ApiResponse> RequestPostAsync<T>(string path, T body, bool useAuthorization = false)
        {
            using (var client = new HttpClient())
            {
                if (useAuthorization)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
                }
                string jsonContent = "";
                if (typeof(T) != typeof(string))
                {
                    jsonContent = JsonConvert.SerializeObject(body);
                }
                else
                {
                    jsonContent = body.ToString();
                }
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(BaseUrl + path, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = new ApiResponse { Body = responseBody, Response = response };
                return apiResponse;
            }
        }

        public async Task<ApiResponse> RequestPutAsync<T>(string path, T? body, bool useAuthorization = false)
        {
            using (var client = new HttpClient())
            {
                if (useAuthorization)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
                }
                string jsonContent = "";
                if (typeof(T) != typeof(string))
                {
                    jsonContent = JsonConvert.SerializeObject(body);
                }
                else
                {
                    jsonContent = body.ToString();
                }
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(BaseUrl + path, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = new ApiResponse { Body = responseBody, Response = response };
                return apiResponse;
            }
        }
	}
}
