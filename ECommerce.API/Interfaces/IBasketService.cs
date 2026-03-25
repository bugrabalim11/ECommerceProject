using ECommerce.API.DTOs.BasketDtos;
using ECommerce.API.Entities;

namespace ECommerce.API.Interfaces
{
    public interface IBasketService
    {
        // YENİ GÖREV: Müşterinin ID'sine göre sepeti getir
        Task<List<Basket>> GetBasketByUserIdAsync(int userId);

        // Generic Repository'den gelen standart işlemler
        Task AddBasketAsync(Basket basket);
        Task DeleteBasketAsync(int id);
        Task UpdateBasketAsync(Basket basket);
    }
}
