using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
