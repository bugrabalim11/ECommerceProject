using ECommerce.MVC.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ECommerce.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 1. Kullanıcı Listesi
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

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

        // 2. Güncelleme Sayfası (Verileri Getiren Kısım)
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API'den ID'ye göre tek bir kullanıcıyı çekiyoruz
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

            // Eğer hala hata alırsan ne olduğunu görmek için:
            return Content($"API Hatası! Durum Kodu: {responseMessage.StatusCode}, Gidilen ID: {id}");
        }

        // 3. Güncelleme İşlemi (Formu Gönderen Kısım)
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonData = JsonSerializer.Serialize(updateUserDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye PUT isteği atıyoruz
            var responseMessage = await client.PutAsync($"https://localhost:7107/api/Users/{updateUserDto.UserId}", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Güncelleme sırasında bir hata oluştu.";
            return View(updateUserDto);
        }

        // 4. Kullanıcı Silme
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync($"https://localhost:7107/api/Users/{id}");
            return RedirectToAction("Index");
        }
    }
}