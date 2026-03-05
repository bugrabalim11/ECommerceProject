using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.UserDtos;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        // Artık veritabanıyla değil, Aşçımızla (Service) konuşuyoruz!
        private readonly IUserService _userService;
        private readonly IMapper _mapper;  // 1. Sihirbazı tanımladık

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;  // 2. İçeri aldık
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            // 1. Aşçıdan (Service) ham verileri (şifreli User listesini) alıyoruz.
            var values = await _userService.GetAllUsersAsync();

            // 2. Sihirbazı devreye sokup bu listeyi GÜVENLİ (şifresiz) DTO listesine çeviriyoruz.
            var mappedValues = _mapper.Map<List<ResultUserDto>>(values);

            // 3. Müşteriye güvenli listeyi sunuyoruz.
            return Ok(mappedValues);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsers(CreateUserDto dto)
        {
            // YENİ VE PROFESYONEL YÖNTEM:
            // AutoMapper sihirbazı, DTO'daki her şeyi alıp User nesnesine şıp diye kopyalıyor!
            var newUser = _mapper.Map<User>(dto);


            // Sadece tarihi manuel veriyoruz çünkü kullanıcının girdiği bir şey değil
            newUser.CreatedAt = DateTime.Now;

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

            // 2. SİHİRBAZIN İKİNCİ YETENEĞİ: Var olan nesneyi güncelleme!
            // Eskiden "existingUser.UserName = dto.UserName" diye tek tek yazdığımız yeri tek satıra düşürdük.
            // Bu kod "dto'nun içindeki yeni bilgileri al, existingUser'ın üzerine yapıştır" demek.
            _mapper.Map(dto, existingUser);

            await _userService.UpdateUserAsync(existingUser);
            return Ok("Kullanıcı Başarıyla Güncellendi!");
        }
    }
}