namespace ECommerce.API.DTOs
{
    public class CreateProductDto
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice {  get; set; }
        public int StockQuantity { get; set; }
    }
}
