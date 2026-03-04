using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        // Artık veritabanıyla değil, Aşçımızla (Service) konuşuyoruz!
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            var values = await _userService.GetAllUsersAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsers(CreateUserDto dto)
        {
            User newUser = new User
            {
                UserName = dto.UserName,
                UserSurname = dto.UserSurname,
                UserEmail = dto.UserEmail,
                UserPassword = dto.UserPassword,
                CreatedAt = DateTime.Now
            };

            await _userService.AddUserAsync(newUser);
            return Ok("Yeni Kullanıcı Başarıyla Eklendi!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("Kullanıcı Başarıyla Silindi!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            // Önce kullanıcı var mı diye servise soruyoruz
            var existingUser = await _userService.GetUserByIdAsync(dto.UserId);
            if (existingUser == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            // Varsa bilgilerini DTO'dan gelenlerle değiştiriyoruz
            existingUser.UserName = dto.UserName;
            existingUser.UserSurname = dto.UserSurname;
            existingUser.UserEmail = dto.UserEmail;
            existingUser.UserPassword = dto.UserPassword;
            // CreatedAt tarihini güncellemiyoruz, ilk kayıt tarihi sabit kalmalı!

            await _userService.UpdateUserAsync(existingUser);
            return Ok("Kullanıcı Başarıyla Güncellendi!");
        }
    }
}