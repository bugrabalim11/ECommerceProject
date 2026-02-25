namespace ECommerce.API.DTOs
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }  //Güncelleme İçin ID Şart
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
    }
}
