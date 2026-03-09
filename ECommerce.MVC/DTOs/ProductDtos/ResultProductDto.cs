namespace ECommerce.MVC.DTOs.ProductDtos
{
    public class ResultProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
