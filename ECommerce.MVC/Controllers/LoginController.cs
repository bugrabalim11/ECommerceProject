using Microsoft.AspNetCore.Mvc;
using ECommerce.MVC.DTOs.LoginDtos;
using System.Text;
using System.Text.Json;

namespace ECommerce.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();  // Boş giriş formunu getirir
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient();

            // Veriyi API'nin anlayacağı JSON formatına çeviriyoruz
            var jsonData = JsonSerializer.Serialize(loginDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'deki Login kapısına (portun 7107 idi) post atıyoruz
            var responseMessage = await client.PostAsync("https://localhost:7107/api/Auth/Login", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                // 1. API'den gelen o kutulu yanıtı alıyoruz
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                // 2. Kutuyu (JSON) açıyoruz
                var jsonDocument = JsonDocument.Parse(jsonString);

                // 3. İçindeki sadece "token" yazan kısmı cımbızla çekiyoruz
                var tokenData = jsonDocument.RootElement.GetProperty("token").GetString();

                // DİKKAT: Sihirli dokunuşlar CookieOptions içine eklendi!
                // Çerezin ADINI ECommerceToken olarak değiştirdik ve ayarları çok sadeleştirdik!
                Response.Cookies.Append("ECommerceToken", tokenData ?? "", new CookieOptions
                {
                    HttpOnly = true,
                    Path = "/",    // Tüm sayfalarda geçerli
                    Expires = DateTime.Now.AddDays(1)
                    // Secure ve SameSite yok! Localhost'ta bize engel olmasınlar.
                });

                return RedirectToAction("Index", "Products");
            }

            // Başarısızsa hata mesajı verip formu tekrar göster
            ModelState.AddModelError("", "Kullanıcı Epostası veya şifre hatalı!");
            return View();
        }
    }
}
