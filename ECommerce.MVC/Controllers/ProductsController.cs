using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ECommerce.MVC.DTOs.ProductDtos;  // Senin DTO'nun olduğu yer

namespace ECommerce.MVC.Controllers
{
    public class ProductsController : Controller
    {
        // 1. Garsonu (HttpClientFactory) tanımlıyoruz
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 2. Sayfa açıldığında çalışacak metot (Index)
        public async Task<IActionResult> Index()
        {
            // Garsonu yarattık
            var client = _httpClientFactory.CreateClient();

            // 🚨 DİKKAT: Buradaki adresi KENDİ API ADRESİN (ve portun) ile değiştir!
            // Örneğin sende https://localhost:7001 veya başka bir şey olabilir. 
            // Swagger'ın adres çubuğuna bakıp doğru portu yazmalısın
            var responseMessage = await client.GetAsync("https://localhost:7107/api/Products");

            // Eğer API "Tamamdır, ürünleri buldum (200 OK)" derse:
            if (responseMessage.IsSuccessStatusCode)
            {
                // Gelen karmaşık JSON metnini oku
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                // JSON metnini bizim o yarattığımız ResultProductDto çantalarına doldur!
                var values = JsonSerializer.Deserialize<List<ResultProductDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // JSON'daki büyük/küçük harf farkını görmezden gel
                });

                // Dolu çantaları Müşteri Salonuna (View'a) gönder!
                return View(values);
            }

            // Eğer API'ye ulaşılamazsa şimdilik boş sayfa döndür
            return View();
        }
    }
}
