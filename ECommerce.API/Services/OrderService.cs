using ECommerce.API.DTOs.OrderDtos;
using ECommerce.API.DTOs.OrderItemDtos;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<ResultOrderDto>> GetOrdersWithDetailsAsync()
        {
            // Controller'dan kopyaladığımız o karmaşık sorgu artık burada, uzman elinde!
            return await _orderRepository.Table // Depocunun tablosuna ulaşıyoruz
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                .ThenInclude(y => y.Product)
                .Select(z => new ResultOrderDto
                {
                    OrderId = z.OrderId,
                    CustomerFullName = z.User.UserName + " " + z.User.UserSurname,
                    OrderDate = z.OrderDate,
                    OrderTotalAmount = z.OrderTotalAmount,
                    OrderStatus = z.OrderStatus,
                    Items = z.OrderItems.Select(w => new ResultOrderItemDto
                    {
                        ProductName = w.Product.ProductName,
                        Quantity = w.Quantity,
                        UnitPrice = w.UnitPrice
                    }).ToList()
                }).ToListAsync();
        }

    }
}