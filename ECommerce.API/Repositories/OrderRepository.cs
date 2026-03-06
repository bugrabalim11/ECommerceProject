using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using Microsoft.EntityFrameworkCore; // Include ve ThenInclude için ŞART!

namespace ECommerce.API.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ECommerceContext context) : base(context)
        {
        }


        // İŞTE YENİ GÜCÜMÜZ: İlişkili tabloları (Join) çekiyoruz!
        public async Task<List<Order>> GetOrdersWithDetailsAsync()
        {
            return await _context.Orders
                .Include(x => x.User) // 1. Siparişi veren Kullanıcıyı dahil et
                .Include(x => x.OrderItems) // 2. Siparişin içindeki Satırları dahil et
                    .ThenInclude(y => y.Product)  // 3. O satırlardaki asıl Ürünleri dahil et
                .ToListAsync(); // Son olarak tüm siparişleri çekiyoruz
        }
    }
}
