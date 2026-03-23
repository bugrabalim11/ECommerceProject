namespace ECommerce.MVC.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
