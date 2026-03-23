using ECommerce.MVC.DTOs.OrderItemDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.MVC.Controllers
{
    [Authorize] // 🔒 Giriş yapmayan sipariş detaylarını kurcalayamaz.
    public class OrderItemsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderItemsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 🟢 HERKES (Giriş Yapan): Kendi siparişinin detayını görebilir.
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id) // Buradaki id, OrderId'dir
        {
            var token = Request.Cookies["ECommerceToken"];
            // [Authorize] sayesinde artık manuel null kontrolüne gerek yok!

            var client = _httpClientFactory.CreateClient();

            // Yaka kartını takıyoruz
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Mutfaktaki yeni uç noktamızdan (Endpoint) sadece bu siparişin ürünlerini istiyoruz!
            var responseMessage = await client.GetAsync($"https://localhost:7107/api/OrderItems/GetItemsByOrderId/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<List<ResultOrderItemDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(values);
            }

            return View(new List<ResultOrderItemDto>());
        }

        // ama Admin tüm kalemleri listelesin istersen burayı da Admin'e kilitleyebilirsin.
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
