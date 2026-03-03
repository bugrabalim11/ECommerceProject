namespace ECommerce.API.DTOs.OrderItemDtos
{
    public class ResultOrderItemDto
    {
        public string ProductName { get; set; } = string.Empty;  // Sadece ürünün adını alacağız
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
