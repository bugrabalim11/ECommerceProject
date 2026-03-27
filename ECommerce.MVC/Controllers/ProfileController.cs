using Microsoft.AspNetCore.Mvc;
using ECommerce.MVC.DTOs.UserDtos; // DTO'larının olduğu klasör
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace ECommerce.MVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // --- 1. PROFİL BİLGİLERİNİ FORMA GETİRME ---
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Cüzdandan Token'ı al
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            // 2. Token'ın içini açıp GERÇEK Kullanıcı ID'sini bul
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdString = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Index", "Login");
            int currentUserId = int.Parse(userIdString);

            // 3. API'ye "Bana bu kullanıcının bilgilerini ver" de
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseMessage = await client.GetAsync($"https://localhost:7107/api/Users/{currentUserId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var userProfile = JsonSerializer.Deserialize<UpdateUserDto>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Veriyi (Eski bilgileri) View'a gönderiyoruz ki form dolu gelsin!
                return View(userProfile);
            }

            return View(new UpdateUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateUserDto dto)
        {
            // 1. Güvenlik: Cüzdanı (Token) kontrol et
            var token = Request.Cookies["ECommerceToken"];
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. Formdan gelen DTO'yu JSON formatına (API'nin anlayacağı dile) çevir
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // 3. API'nin kapısını çal!
            // DİKKAT: API tarafındaki metodun [HttpPut("{id}")] olduğu için URL'nin sonuna ID'yi ekliyoruz.
            var responseMessage = await client.PutAsync($"https://localhost:7107/api/Users/{dto.UserId}", content);

            // 4. Eğer işlem başarılıysa Ana Sayfaya yönlendir
            if (responseMessage.IsSuccessStatusCode)
            {
                // Yönlendirmeden hemen önce mesajı TempData'nın cebine koyuyoruz
                // Dİğer tüm başarılı işlemlerde kullanılabilir !!!
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi!";
                return RedirectToAction("Index", "Home");
            }

            // Eğer bir hata çıkarsa, kullanıcının girdiği veriler kaybolmasın diye formu tekrar aynı bilgilerle aç
            return View(dto);
        }
    }
}
