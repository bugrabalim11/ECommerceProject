using ECommerce.API.Entities;
using ECommerce.API.Interfaces;

namespace ECommerce.API.Services
{
    public class OrderService : IOrderService
    {
        // 1. C#'a diyoruz ki: "Benim _orderRepository adında gizli (private) bir depocum olacak."
        private readonly IOrderRepository _orderRepository;

        // 2. Yapıcı Metot (Constructor): Dışarıdan gelen depocuyu, içerideki gizli depocuya eşitliyoruz.
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _orderRepository.Update(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order != null)
            {
                _orderRepository.Delete(order);
            }
        }


        // --- DİĞER İKİ METODU SİLİP SADECE BUNU BIRAKIYORUZ ---
        public async Task<List<Order>> GetOrdersWithDetailsAsync()
        {
            return await _orderRepository.GetOrdersWithDetailsAsync();
        }
    }
}