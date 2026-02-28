namespace ECommerce.API.DTOs.OrderDtos
{
    public class ResultOrderDto
    {
        public int OrderId { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;  // // Müşterinin adını ve soyadını birleştirip vereceğiz
        public DateTime OrderDate { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;



        // Siparişin içindeki ürünleri liste olarak tutacağımız yer:
        // Liste için uyarıyı çözen O SİHİRLİ KOD: = new(); (Boş bir liste başlatır)
        public List<ResultOrderItemDto> Items { get; set; } = new();
    }
}
