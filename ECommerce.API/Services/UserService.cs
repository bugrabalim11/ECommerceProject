using ECommerce.API.Interfaces;
using ECommerce.API.Entities;

namespace ECommerce.API.Services
{
    // CS0535 hatası almamak için IUserService sözleşmesindeki tüm maddeleri buraya yazıyoruz
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        // Aşçımız, Kullanıcı Depocusunu tanıyor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        // İşte o özel metodumuz! Veritabanından email ile kullanıcı bulma işi
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }
    }
}