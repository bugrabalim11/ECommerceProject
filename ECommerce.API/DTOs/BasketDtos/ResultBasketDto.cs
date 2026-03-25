namespace ECommerce.API.DTOs.BasketDtos
{
    public class ResultBasketDto
    {
        public int BasketId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal ProductPrice { get; set; }

        // Sepette ürünün ID'si değil, adı yazsın diye ekstra bir alan ekleyebiliriz:
        public string ProductName { get; set; } = string.Empty;
    }
}
