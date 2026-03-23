using ECommerce.API.Interfaces;
using ECommerce.API.DTOs.LoginDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.API.Services
{
    public class AuthService : IAuthService
    {
        // Güvenlik Şefinin iki şeye ihtiyacı var:
        // 1. İçerideki Kullanıcı Aşçısına (Veritabanında bu adam var mı diye sormak için)
        private readonly IUserService _userService;


        // 2. Ayar dosyasına (appsettings.json içindeki gizli şifreleri okumak için)
        private readonly IConfiguration _configuration;


        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }


        public async Task<string?> LoginAsync(LoginDto loginDto) // **** ? sana null da dönebilirim demek ****
        {
            // ADIM 1: Aşçıya sor -> "Bu maile sahip biri var mı?"
            var user = await _userService.GetUserByEmailAsync(loginDto.UserEmail);


            // ADIM 2: Kontrol Et -> Adam yoksa veya şifresi uyuşmuyorsa kapı dışarı et!
            if (user == null || user.UserPassword != loginDto.UserPassword)
            {
                return null; // Boş dönüyoruz, Controller bunu görünce "Hatalı Giriş" diyecek.
            }


            // ADIM 3: Kimlik Doğrulandı! VIP Bilekliğini (Token) Üretme Zamanı


            // a) appsettings.json'daki gizli anahtarımızı (Key) alıp kilit oluşturuyoruz
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)); // ****** ! null işareti koyfuk çünkü ayar dosyasnı kendi elimizle yazdık şifrenin orda olduğundan eminiz ****
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            // b) Token'ın içine koyacağımız bilgiler (Adı, ID'si vs.)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.UserEmail),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), // Token'a özel benzersiz ID
                new Claim("UserId", user.UserId.ToString()), // İleride sepete ürün eklerken bu ID çok lazım olacak!

                // 👑 GÜNCELLENEN SATIR: Eğer user.Role boş (null) gelirse, otomatik "User" kabul et!
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };


            // c) Bilekliği basıyoruz! (Kim üretti, kime üretti, ne zaman süresi bitecek)
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"], // Bilekliği kim üretti?
                audience: _configuration["JwtSettings:Audience"], // Bilekliği kim kullanacak?
                claims: claims, // Bilekliğin içine koyduğumuz bilgiler
                expires: DateTime.Now.AddHours(1), // Bilekliğin geçerlilik süresi
                signingCredentials: credentials // Bilekliği imzalayan anahtar
            );


            // d) Oluşan bilekliği yazı (string) formatına çevirip teslim ediyoruz
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
