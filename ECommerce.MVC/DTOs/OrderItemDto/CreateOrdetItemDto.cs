namespace ECommerce.MVC.DTOs.OrderItemDto
{
    public class CreateOrdetItemDto
    {
        public int OrderId { get; set; }  
        public int ProductId { get; set; }  
        public int Quantity { get; set; }  
        public decimal UnitPrice { get; set; }   
    }
}
