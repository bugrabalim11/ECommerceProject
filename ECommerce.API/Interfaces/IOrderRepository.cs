using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // YENİ GÖREV: Siparişleri detaylarıyla (Kullanıcı ve Ürünler) birlikte getir!
        Task<List<Order>> GetOrdersWithDetailsAsync();

    }
}
