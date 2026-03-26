using AutoMapper;
using ECommerce.API.Interfaces;
using ECommerce.API.DTOs.LoginDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.API.Entities; // User sınıfı için

namespace ECommerce.API.Services
{
    public class AuthService : IAuthService
    {
        // Güvenlik Şefinin iki şeye ihtiyacı var:
        // 1. İçerideki Kullanıcı Aşçısına (Veritabanında bu adam var mı diye sormak için)
        private readonly IUserService _userService;

        // 2. Ayar dosyasına (appsettings.json içindeki gizli şifreleri okumak için)
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;  // 👈 1. Mapper'ı tanımladık


        public AuthService(IUserService userService, IConfiguration configuration, IMapper mapper)
        {
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
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
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // İleride sepete ürün eklerken bu çok lazım olacak!

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


        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // ADIM 1: Bu mail adresiyle daha önce kayıt olunmuş mu kontrol et!
            var existingUser = await _userService.GetUserByEmailAsync(registerDto.UserEmail);

            if (existingUser != null)
            {
                return false; // E-posta zaten var, aynı maille iki kere kayıt olunmaz!
            }

            // 🛑 MANUEL EŞLEŞTİRME YERİNE AUTOMAPPER KULLANIYORUZ:
            // "Çantadaki (DTO) verileri al, doğrudan User nesnesine dönüştür"
            var newUser = _mapper.Map<User>(registerDto);

            // DTO'da olmayan ama veritabanının istediği zorunlu alanları elle ekliyoruz:
            newUser.CreatedAt = DateTime.Now;
            newUser.Role = "User";

            // Aşçıya gönderiyoruz
            await _userService.AddUserAsync(newUser);

            // 🛑 İŞTE CS0161 HATASINI ÇÖZEN SATIR BURASI:
            // Kod buraya kadar hatasız geldiyse, "Kayıt Başarılı (true)" diyerek işlemi bitirmeliyiz!
            return true;
        }
    }
}
