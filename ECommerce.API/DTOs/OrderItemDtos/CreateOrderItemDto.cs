namespace ECommerce.API.DTOs.OrderItemDtos
{
    public class CreateOrderItemDto
    {
        // Id yok, veritabanı otomatik verecek.
        public int OrderId { get; set; }  // Hangi siparişin detayı
        public int ProductId { get; set; }  // Hangi ürün alındı
        public int Quantity { get; set; }   // Kaç adet alındı
        public decimal UnitPrice { get; set; }   // O anki satış fiyatı neydi
    }
}
