namespace ECommerce.API.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        // OrderId, OrderDate ve OrderStatus yok! Onları arka planda biz halledeceğiz.

        public int UserId { get; set; }
        public decimal OrderTotalAmount { get; set; }
    }
}
