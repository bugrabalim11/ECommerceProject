namespace ECommerce.API.Entities
{
    public class Basket
    {
        public int BasketId { get; set; }

        // 1. Bu sepetteki ürün KİMİN? (Müşteri ID'si)
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // 2. Bu sepette HANGİ ÜRÜN var? (Ürün ID'si)
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // 3. Bu üründen KAÇ TANE aldı?
        public int StockQuantity { get; set; }

        // 4. Sepete eklendiği andaki FİYATI nedir?
        public decimal ProductPrice { get; set; }

    }
}
