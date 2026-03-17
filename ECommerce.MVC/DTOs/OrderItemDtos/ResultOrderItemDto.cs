namespace ECommerce.MVC.DTOs.OrderItemDtos
{
    public class ResultOrderItemDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // JSON'daki o "product" kutusunu yakalamak için ekliyoruz:
        public OrderItemProductDto Product { get; set; }
    }

    // O küçük kutunun içindeki "productName" bilgisini alacak sınıfımız:
    public class OrderItemProductDto
    {
        public string ProductName { get; set; } = string.Empty;
    }
}
