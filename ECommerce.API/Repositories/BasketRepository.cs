using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories
{
    // GenericRepository'den miras alarak Ekle/Sil/Güncelle'yi bedavaya getirdik!
    public class BasketRepository : GenericRepository<Basket>, IBasketRepository
    {
        // Generic Repository'nin Constructor'ı bizden Context istediği için ona gönderiyoruz (base)
        public BasketRepository(ECommerceContext context) : base(context)
        {
        }

        public async Task<List<Basket>> GetBasketByUserIdAsync(int userId)
        {
            // _context, GenericRepository'den protected olarak miras geldiği için burada kullanabiliyoruz
            return await _context.Baskets
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}