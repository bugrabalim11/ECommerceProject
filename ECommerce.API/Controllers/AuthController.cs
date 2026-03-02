using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.API.DTOs.LoginDtos;
using ECommerce.API.Entities;
using ECommerce.API.DTOs;
using ECommerce.API.Context;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ECommerceContext _context;  // Veritabanı bağlantımız
        private readonly IConfiguration _configuration;  // appsettings'i okumak için

        public AuthController(ECommerceContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            // 1. KAPI KONTROLÜ: Veritabanında böyle bir kullanıcı var mı? (Adı ve Şifresi eşleşiyor mu?)
            var user = _context.Users.FirstOrDefault(x => x.UserName == loginDto.UserName && x.UserPassword == loginDto.UserPassword);

            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı!");
            }

            // 2. KARTIN İÇİNE YAZILACAKLAR: Kullanıcıyı bulduk. Kartın (Token) içine kimlik bilgilerini yazıyoruz.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Kullanıcının ID'si
                new Claim(ClaimTypes.Name, user.UserName), // Kullanıcının Adı
                new Claim(ClaimTypes.Role, "Admin") // Şimdilik herkese "Admin" yetkisi veriyoruz!
            };


            // 3. ISLAK İMZA: appsettings.json içindeki gizli şifremizi alıp kalemi hazırlıyoruz.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwtsettings:secretkey").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // 4. KARTI BAS: Bütün ayarları birleştirip o uzun karmaşık Token'ı üretiyoruz.
            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("jwtsettings:Issuer").Value,
                audience: _configuration.GetSection("jwtsettings:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token 1 saat geçerli olacak
                signingCredentials: creds
            );


            // 5. KARTI TESLİM ET: Üretilen Token'ı yazıya çevirip kullanıcıya ver.
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
