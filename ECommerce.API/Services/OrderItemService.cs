using ECommerce.API.Entities;
using ECommerce.API.Interfaces;


namespace ECommerce.API.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _orderItemRepository.GetAllAsync();
        }
        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            return await _orderItemRepository.GetByIdAsync(id);
        }
        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _orderItemRepository.AddAsync(orderItem);
        }
        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _orderItemRepository.Update(orderItem);
        }
        public async Task DeleteOrderItemAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem != null)
            {
                _orderItemRepository.Delete(orderItem);
            }
        }
    }
}
