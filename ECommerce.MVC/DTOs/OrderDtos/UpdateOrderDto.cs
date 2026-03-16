namespace ECommerce.MVC.DTOs.OrderDtos
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }  
        public int UserId { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
    }
}
