using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    // Kategori deposu, IGenericRepository'deki bütün kuralları (Category için) miras alır!
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }
}