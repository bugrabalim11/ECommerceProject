namespace ECommerce.MVC.DTOs.BasketDtos
{
    public class CreateBasketDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
