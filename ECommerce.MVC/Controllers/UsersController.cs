using ECommerce.MVC.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization; // 👈 1. Kütüphaneyi ekledik

namespace ECommerce.MVC.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 2. TÜM SINIFI MÜHÜRLEDİK! (Sadece Admin girebilir)
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 1. Kullanıcı Listesi
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["ECommerceToken"];
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.GetAsync("https://localhost:7107/api/Users");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<List<ResultUserDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(values);
            }
            return View(new List<ResultUserDto>());
        }

        // 2. Güncelleme Sayfası
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var token = Request.Cookies["ECommerceToken"];
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Users/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<UpdateUserDto>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                return View(values);
            }
            return RedirectToAction("Index");
        }

        // 3. Güncelleme İşlemi
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var token = Request.Cookies["ECommerceToken"];
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonData = JsonSerializer.Serialize(updateUserDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync($"https://localhost:7107/api/Users/{updateUserDto.UserId}", stringContent);

            return responseMessage.IsSuccessStatusCode ? RedirectToAction("Index") : View(updateUserDto);
        }

        // 4. Kullanıcı Silme
        public async Task<IActionResult> DeleteUser(int id)
        {
            var token = Request.Cookies["ECommerceToken"];
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"https://localhost:7107/api/Users/{id}");
            return RedirectToAction("Index");
        }
    }
}