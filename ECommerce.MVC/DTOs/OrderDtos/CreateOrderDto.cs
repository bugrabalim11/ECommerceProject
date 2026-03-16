namespace ECommerce.MVC.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public decimal OrderTotalAmount { get; set; }
    }
}
