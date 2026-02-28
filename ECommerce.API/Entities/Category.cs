namespace ECommerce.API.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;



        // Bir kategorinin birden fazla ürünü olabilir
        public ICollection<Product>? Products { get; set; } = null!;
    }
}
