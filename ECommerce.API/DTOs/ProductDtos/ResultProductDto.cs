namespace ECommerce.API.DTOs.ProductDtos
{
    public class ResultProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }


        // DİKKAT: CategoryId veya koca Category nesnesi yerine, 
        // SADECE kategorinin adını alacağız! Mimari güzellik buradadır.
        public string CategoryName { get; set; } = string.Empty;
    }
}
