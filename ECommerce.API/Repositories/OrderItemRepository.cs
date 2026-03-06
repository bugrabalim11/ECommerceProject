using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;  //Include için gerekli 

namespace ECommerce.API.Repositories
{
    public class OrderItemRepository:GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ECommerceContext context) : base(context)
        {
        }


        public async Task<List<OrderItem>> GetOrderItemsWithProductAsync()
        {
            // Veritabanından satırları çekerken, içindeki Product (Ürün) tablosunu da JOIN yap (Dahil et)
            return await _context.OrderItems.Include(x => x.Product).ToListAsync();
        }
    }
}
