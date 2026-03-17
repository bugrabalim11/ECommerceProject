using ECommerce.MVC.DTOs.OrderItemDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ECommerce.MVC.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderItemsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id) // Buradaki id, OrderId'dir
        {
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

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


        public IActionResult Index()
        {
            return View();
        }
    }
}
