using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    // Sepet depomuz, Genel depomuzun (IGenericRepository) tüm yeteneklerine bedavadan sahip oluyor!
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        // YENİ GÖREV: Müşterinin sepetini, ürün bilgileriyle (Join/Include) birlikte getir.
        Task<List<Basket>> GetBasketByUserIdAsync(int userId);
    }
}
