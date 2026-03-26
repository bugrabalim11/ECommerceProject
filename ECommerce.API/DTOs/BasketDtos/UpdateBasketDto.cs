namespace ECommerce.API.DTOs.BasketDtos
{
    public class UpdateBasketDto
    {
        public int BasketId { get; set; }
        public int StockQuantity { get; set; } // Sadece adeti güncelleyeceğiz
    }
}
