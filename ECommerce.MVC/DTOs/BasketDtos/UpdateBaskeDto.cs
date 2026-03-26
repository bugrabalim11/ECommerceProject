namespace ECommerce.MVC.DTOs.BasketDtos
{
    public class UpdateBaskeDto
    {
        public int BasketId { get; set; } // Hangi sepet satırını güncelleyeceğiz? (ID şart!)
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; } // Müşteri muhtemelen burayı değiştirecek (Adet artırma/azaltma)
        public decimal ProductPrice { get; set; }
    }
}
