using ECommerce.API.DTOs.LoginDtos;
using ECommerce.API.DTOs.UserDtos;

namespace ECommerce.API.Interfaces
{
    public interface IAuthService
    {
        // Geriye o uzun şifreli metni (Token) döneceği için Task<string> yazıyoruz.
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}
