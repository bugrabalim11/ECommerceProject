namespace ECommerce.API.DTOs.CategoryDtos
{
    public class ResultCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        // DİKKAT: Products listesini buraya koymuyoruz!
    }
}
