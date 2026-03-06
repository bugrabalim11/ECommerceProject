using ECommerce.API.DTOs.OrderDtos;
using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    // AŞÇININ SÖZLEŞMESİ (SERVICE INTERFACE)
    // Bu arayüz, Sipariş (Order) işlemleri için dışarıya (Controller'a) hangi hizmetleri sunacağımızı (menümüzü) belirler.
    public interface IOrderService
    {
        // 1. Veritabanındaki tüm siparişleri liste halinde getirme sözü.
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        // 2. Verilen ID'ye göre tek bir siparişi getirme sözü.
        // DİKKAT (?): Soru işareti, "Eğer o ID'de bir sipariş bulamazsam geriye boş (null) dönerim, programı çökertmem" demektir.
        Task<Order?> GetOrderByIdAsync(int id);

        // 3. Yeni bir siparişi alıp veritabanına ekleme sözü.
        Task AddOrderAsync(Order order);

        // 4. Var olan bir siparişin bilgilerini güncelleme sözü.
        Task UpdateOrderAsync(Order order);

        // 5. Sadece ID'sini vererek bir siparişi veritabanından tamamen silme sözü.
        Task DeleteOrderAsync(int id);

        // AŞÇININ YENİ GÖREVİ:
        Task<List<Order>> GetOrdersWithDetailsAsync();
    }
}