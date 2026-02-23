namespace ECommerce.API.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; }


        // İlişki: Bu ürün hangi kategoriye ait?
        public Category Category { get; set; } = null!;

    }
}
