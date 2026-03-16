using ECommerce.MVC.DTOs.OrderDtos; // Kendi DTO klasör yolunu buraya yazmayı unutma
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ECommerce.MVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. GÜVENLİK KONTROLÜ: Garsonun elinde geçerli bir yaka kartı (Token) var mı?
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token))
            {
                // Kart yoksa kapı dışarı (Login sayfasına)
                return RedirectToAction("Index", "Login");
            }

            // 2. HTTP İSTEMCİSİ OLUŞTUR: Garsonu mutfağa (API) yollamak için hazırlıyoruz
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3. API'DEN VERİYİ İSTE: Şeften tüm siparişlerin listesini istiyoruz
            var responseMessage = await client.GetAsync("https://localhost:7107/api/Orders");

            if (responseMessage.IsSuccessStatusCode)
            {
                // 4. GELEN PAKETİ AÇ (JSON -> DTO ÇEVİRİSİ)
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                var values = JsonSerializer.Deserialize<List<ResultOrderDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Büyük/küçük harf duyarlılığını kapat (API Name gönderir, biz name okuruz sorun olmaz)
                });

                // 5. DOLU ÇANTALARI VİTRİNE (VIEW) GÖNDER
                return View(values);
            }

            // Eğer API'den olumsuz bir yanıt gelirse (veya sipariş yoksa) sayfaya boş liste gönder ki sayfa patlamasın
            return View(new List<ResultOrderDto>());
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.DeleteAsync($"https://localhost:7107/api/Orders/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Tıklanan siparişin ID'sine göre API'den sadece o siparişi getir
            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Orders/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();


                // Gelen veriyi UpdateOrderDto'ya çevir
                var value = JsonSerializer.Deserialize<UpdateOrderDto>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(value);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var token = Request.Cookies["ECommerceJwt"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonData = JsonSerializer.Serialize(updateOrderDto);
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            // API'deki [HttpPut] metoduna paketi gönderiyoruz
            var responseMessage = await client.PutAsync("https://localhost:7107/api/Orders", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(updateOrderDto);
        }

    }
}