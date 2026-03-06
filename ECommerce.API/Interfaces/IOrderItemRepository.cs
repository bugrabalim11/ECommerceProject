using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    // GenericRepository sayesinde Ekle/Sil/Listele yetenekleri otomatik geliyor.
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        // Ürün bilgisiyle beraber sipariş detaylarını getir
        Task<List<OrderItem>> GetOrderItemsWithProductAsync();
    }
}
