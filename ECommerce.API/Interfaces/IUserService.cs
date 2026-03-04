using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);  // Tek bir kullanıcıyı görmek için
        Task<User?> GetUserByEmailAsync(string email);  // Giriş (Login) yaparken lazım olacak!
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(User user);



        // Kayıt olma (Register) işlemini UserService mi yapmalı yoksa AuthService mi? 
        // Genelde AuthService üzerinden UserService'e "Kullanıcıyı Kaydet" emri verilir.
        Task AddUserAsync(User user);
    }
}
