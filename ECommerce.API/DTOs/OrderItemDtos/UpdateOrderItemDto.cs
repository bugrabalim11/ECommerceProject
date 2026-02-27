namespace ECommerce.API.DTOs.OrderItemDtos
{
    public class UpdateOrderItemDto
    {
        public int Id { get; set; }  //Güncelleme yapmak için
        public int OrderId { get; set; }  
        public int ProductId { get; set; }  
        public int Quantity { get; set; }   
        public decimal UnitPrice { get; set; }   
    }
}
