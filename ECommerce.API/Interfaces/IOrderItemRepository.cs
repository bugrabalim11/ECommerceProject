using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    // GenericRepository sayesinde Ekle/Sil/Listele yetenekleri otomatik geliyor.
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        
    }
}
