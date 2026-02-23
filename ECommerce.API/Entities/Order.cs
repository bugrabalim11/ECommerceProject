namespace ECommerce.API.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;


        // İlişki: Bu sipariş hangi kullanıcıya ait?
        public User User { get; set; } = null!;

        // İlişki (Bire-Çok): Bir siparişin içinde birden fazla ürün detayı olabilir
        public ICollection<OrderItem> OrderItems { get; set; } = null!;
    }
}
