using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Interfaces;
using ECommerce.API.DTOs.LoginDtos; // DTO klasörünün adını LoginDtos yapmıştın

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Garson artık sadece Güvenlik Şefini (AuthService) tanıyor, veritabanını değil!
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // 1. Müşterinin verdiği Email ve Şifreyi Güvenlik Şefine yolla
            var token = await _authService.LoginAsync(loginDto);

            // 2. Eğer Şef null (boş) dönerse, demek ki adam kayıtlı değil veya şifre yanlış
            if (token == null)
            {
                return Unauthorized("Giriş başarısız: Email veya şifre hatalı!"); // 401 Hatası
            }

            // 3. Her şey doğruysa, üretilen Token'ı (VIP Bilekliği) müşteriye ver
            return Ok(new { Token = token, Message = "Giriş Başarılı!" });
        }
    }
}