using ECommerce.MVC.DTOs.BasketDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;  // Token okuyucu
using System.Net.Http.Headers;
using System.Security.Claims;   // Claim (Hak) türleri
using System.Text;
using System.Text.Json;

namespace ECommerce.MVC.Controllers
{
    [Authorize]
    public class BasketsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BasketsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // --- 1. SEPETİM SAYFASI (Listeleme) ---
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Cüzdandan (Cookie) anahtarı (Token) al
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            // 🌟 İŞTE GERÇEK KOD: Token'ı açıp içindeki UserId'yi buluyoruz!
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = new JwtSecurityToken(token);
            var userIdString = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            // Eğer Token'dan ID gelmezse (güvenlik) login'e at
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Index", "Login");

            int currentUserId = int.Parse(userIdString);
            // 🌟 ---------------------------------------------------------

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Artık Token'dan gelen GERÇEK ID'yi API'ye gönderiyoruz
            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Baskets/{currentUserId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize<List<ResultBasketDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(values);
            }

            return View(new List<ResultBasketDto>());
        }

        // --- 2. SEPETE YENİ ÜRÜN EKLEME ---
        [HttpPost]
        public async Task<IActionResult> CreateBasket(int productId, decimal price)
        {
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            // 🌟 YİNE GERÇEK KOD: Ekleyen kişinin ID'sini Token'dan alıyoruz
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdString = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Index", "Login");
            int currentUserId = int.Parse(userIdString);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateBasketDto createBasketDto = new CreateBasketDto
            {
                UserId = currentUserId, // 👈 Artık sabit 1 değil, gerçek ID!
                ProductId = productId,
                StockQuantity = 1,
                ProductPrice = price
            };

            var jsonData = JsonSerializer.Serialize(createBasketDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7107/api/Baskets", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Products");
            }

            return RedirectToAction("Index", "Products");
        }

        // --- 3. SEPETTEN ÜRÜN SİLME ---
        [HttpGet]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.DeleteAsync($"https://localhost:7107/api/Baskets/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateQuantity(int id, int newQuantity)
        {
            // 1. Güvenlik ve Mantık Kontrolü
            if (newQuantity < 1) return RedirectToAction("Index");

            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. DTO Hazırlama (Senin sadeleştirdiğin UpdateBasketDto yapısına uygun)
            var updateDto = new
            {
                BasketId = id,
                StockQuantity = newQuantity
            };

            // 3. JSON'a çevirme (Senin projenin kullandığı System.Text.Json ile)
            var jsonData = JsonSerializer.Serialize(updateDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // 4. API'ye Gönderim
            var responseMessage = await client.PutAsync("https://localhost:7107/api/Baskets", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
