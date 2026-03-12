namespace ECommerce.MVC.DTOs.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
    }
}
