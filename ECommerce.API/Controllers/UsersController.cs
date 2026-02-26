using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.UserDtos;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public UsersController(ECommerceContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetUserList()
        {
            var values = _context.Users.ToList();
            return Ok(values);
        }


        [HttpPost]
        public IActionResult CreateUsers(CreateUserDto dto)
        {
            User newUser = new User
            {
                UserName = dto.UserName,  //Süslü parantez { } içinde liste şeklinde özellik belirtiyorsan: Virgül ( , )
                UserSurname = dto.UserSurname,
                UserEmail = dto.UserEmail,
                UserPassword = dto.UserPassword,
                CreatedAt = DateTime.Now  // Tarihi dışarıdan almadık, tam şu anki zamanı otomatik atadık!
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok("Yeni Kullanıcı Başarıyla Eklendi!");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var value = _context.Users.Find(id);
            if (value == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }
            _context.Users.Remove(value);
            _context.SaveChanges();
            return Ok("Kullanıcı Başarıyla Silindi!");
        }

        [HttpPut]
        public IActionResult UpdatedUser(UpdateUserDto dto)
        {
            var value = _context.Users.Find(dto.UserId);
            if (value == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }
            value.UserName = dto.UserName;  //Alt alta tekil, bağımsız komutlar veriyorsan: Noktalı Virgül ( ; )
            value.UserSurname = dto.UserSurname;
            value.UserEmail = dto.UserEmail;
            value.UserPassword = dto.UserPassword;
            // CreatedAt tarihini güncellemiyoruz, ilk kayıt tarihi sabit kalmalı!

            _context.SaveChanges();
            return Ok("Kullanıcı Başarıyla Güncellendi!");
        }
    }
}
