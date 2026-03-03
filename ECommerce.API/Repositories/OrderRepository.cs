using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;

namespace ECommerce.API.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ECommerceContext context) : base(context)
        {
        }
    }
}
