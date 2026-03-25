using ECommerce.API.Entities;
using ECommerce.API.Interfaces;

namespace ECommerce.API.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<List<Basket>> GetBasketByUserIdAsync(int userId)
        {
            // Özel sorgumuzu repo'dan çağırıyoruz
            return await _basketRepository.GetBasketByUserIdAsync(userId);
        }

        public async Task AddBasketAsync(Basket basket)
        {
            // Senin IGenericRepository'deki AddAsync metodun
            await _basketRepository.AddAsync(basket);
        }

        public async Task UpdateBasketAsync(Basket basket)
        {
            // Senin IGenericRepository'deki Update metodun (Entity alıyor)
            _basketRepository.Update(basket);
        }

        public async Task DeleteBasketAsync(int id)
        {
            // Tıpkı ProductService'te yaptığın gibi: Önce bul, sonra sil!
            var basket = await _basketRepository.GetByIdAsync(id);

            if (basket != null)
            {
                // Senin IGenericRepository'deki Delete metodun (Entity alıyor)
                _basketRepository.Delete(basket);
            }
        }
    }
}