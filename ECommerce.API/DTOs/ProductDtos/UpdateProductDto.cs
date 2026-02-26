namespace ECommerce.API.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        public int CategoryId { get; set; } // Hangi kategoriye ait olduğunu belirtmek için CategoryId ekledik
        public int ProductId { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
