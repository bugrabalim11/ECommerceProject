namespace ECommerce.MVC.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty; // API ile uyumlu hale getirdik
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; } // API ile uyumlu hale getirdik
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; } // 'I' harfine dikkat! API'deki gibi yaptık.
    }
}

