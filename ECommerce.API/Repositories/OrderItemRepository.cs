using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;

namespace ECommerce.API.Repositories
{
    public class OrderItemRepository:GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ECommerceContext context) : base(context)
        {
        }
    }
}
