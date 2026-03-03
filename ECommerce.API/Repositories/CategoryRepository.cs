using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;


namespace ECommerce.API.Repositories
{
    // CategoryRepository, GenericRepository'nin tüm özelliklerini alır ve ICategoryRepository'e uyar.
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ECommerceContext context) : base(context)
        {
        }
    }
}