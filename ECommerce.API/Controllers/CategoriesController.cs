using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize] // Dün astığımız kapı kilidimiz :)
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // Artık veritabanını (Context) DEĞİL, Aşçımızı (Service) çağırıyoruz!
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous] // Vitrinimiz herkese açık
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            // Controller'ın içi ne kadar temizleşti bak: "Aşçı bana tüm kategorileri ver!"
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound("Bu ID'ye ait kategori bulunamadı.");

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _categoryService.AddCategoryAsync(category);
            return Ok("Kategori başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            await _categoryService.UpdateCategoryAsync(category);
            return Ok("Kategori başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok("Kategori başarıyla silindi.");
        }
    }
}
