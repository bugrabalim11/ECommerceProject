using ECommerce.MVC.DTOs.OrderDtos;
using ECommerce.MVC.DTOs.OrderItemDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.MVC.Controllers
{
    [Authorize] // 🔒 2. Giriş yapmayan bu kapıdan içeri bakamaz bile!
    public class OrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 🛑 SADECE ADMİN YAZAN KİLİDİ SİLDİK: Artık giriş yapan herkes siparişlerim sayfasına girebilir.
        // [Authorize(Roles = "Admin")] -> BUNU TAMAMEN SİL
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. GÜVENLİK KONTROLÜ: Garsonun elinde geçerli bir yaka kartı (Token) var mı?
            var token = Request.Cookies["ECommerceToken"];
            

            // 2. HTTP İSTEMCİSİ OLUŞTUR: Garsonu mutfağa (API) yollamak için hazırlıyoruz
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3. API'DEN VERİYİ İSTE: Şeften tüm siparişlerin listesini istiyoruz
            var responseMessage = await client.GetAsync("https://localhost:7107/api/Orders/GetOrderWithDetails");

            // 🛡️ YENİ GÜVENLİK KONTROLÜ: Şef (API) "Bu kart geçersiz veya süresi dolmuş" derse (401):
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Response.Cookies.Delete("ECommerceToken"); // Bozuk/Süresi dolmuş kartı çöpe at
                return RedirectToAction("Index", "Login"); // Login'e postala!
            }

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

        // 🛑 SADECE ADMİN: Sipariş iptali/silme
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.DeleteAsync($"https://localhost:7107/api/Orders/{id}");

            

            return RedirectToAction("Index");
        }

        // 🛑 SADECE ADMİN: Sipariş durumunu güncelleme (Kargoya verildi vs.)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var token = Request.Cookies["ECommerceToken"];
            

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var token = Request.Cookies["ECommerceToken"];
            

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

        // 🟢 HERKES (Giriş Yapan): Kendi siparişinin detayına bakabilir 
        // (Şimdilik Admin'e kilitli kalabilir veya her üyeye açılabilir)
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            // 1. Cüzdandan yeni yaka kartımızı alıyoruz
            var token = Request.Cookies["ECommerceToken"];

            

            // 3. Garsonu mutfağa gönderiyoruz
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.GetAsync($"https://localhost:7107/api/OrderItems/GetItemsByOrderId/{id}");

            // 4. Şef (API) "Bu kartın süresi dolmuş" derse:
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Response.Cookies.Delete("ECommerceToken"); // Çürük kartı çöpe at
                return RedirectToAction("Index", "Login"); // Login'e postala
            }

            // 5. Her şey başarılıysa, o 20 buzdolabını vitrine yolla!
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<List<ResultOrderItemDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(values);
            }

            // 6. Başka bir hata olursa sayfa çökmesin, boş liste gitsin
            return View(new List<ResultOrderItemDto>());
        }

    }
}