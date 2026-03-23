using Microsoft.AspNetCore.Mvc;
using ECommerce.MVC.DTOs.LoginDtos;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
// --- YENİ EKLENENLER ---
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var jsonData = JsonSerializer.Serialize(loginDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7107/api/Auth/Login", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(jsonString);
                var tokenData = jsonDocument.RootElement.GetProperty("token").GetString();

                // 🛑 1. ADIM: Token'ı parçalayıp içindeki Rol ve İsim bilgilerini okuyoruz
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tokenData);
                var claims = jwtToken.Claims.ToList(); // Token içindeki tüm iddiaları (role, name vb.) aldık

                // 🛑 2. ADIM: MVC'nin anlayacağı bir kimlik (Identity) oluşturuyoruz
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Tarayıcı kapanınca hemen silinmesin
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                };

                // 🛑 3. ADIM: MVC'ye "Bu adamı içeri al ve kimliğini tanı" diyoruz
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                // Eski yaptığın manuel cookie ekleme kalsın, API isteklerinde kullanıyoruz
                Response.Cookies.Append("ECommerceToken", tokenData ?? "", new CookieOptions
                {
                    HttpOnly = true,
                    Path = "/",
                    Expires = DateTime.Now.AddDays(1)
                });

                return RedirectToAction("Index", "Products");
            }

            ModelState.AddModelError("", "Kullanıcı Epostası veya şifre hatalı!");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // 1. .NET'in kendi oluşturduğu "Ben kimim?" çerezini siler
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Senin API için oluşturduğun JWT Token çerezini siler
            Response.Cookies.Delete("ECommerceToken");

            // 3. Kullanıcıyı ana sayfaya veya Login ekranına gönder
            return RedirectToAction("Index", "Login");
        }
    }
}
