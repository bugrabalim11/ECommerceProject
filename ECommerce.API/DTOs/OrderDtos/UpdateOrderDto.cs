namespace ECommerce.API.DTOs.OrderDtos
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }  // Güncelleme için şart
        public int UserId { get; set; }
        public decimal OrderTotalAmount { get; set; }


        // Güncellerken kargoya verildi, iptal edildi gibi durumları girebilmek için bunu alıyoruz.
        public string OrderStatus { get; set; } = string.Empty;
        // Sipariş tarihini (OrderDate) güncellemeyiz, o ilk verildiği an olarak kalmalı.
    }
}
