using AutoMapper;
using ECommerce.API.DTOs.CategoryDtos;
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
        private readonly IMapper _mapper; // 1. Sihirbazı tanımladık

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;  // İçeri aldık
        }

        [AllowAnonymous] // Vitrinimiz herkese açık
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var values = await _categoryService.GetAllCategoriesAsync();  
            var mappedValues = _mapper.Map<List<ResultCategoryDto>>(values);  
            return Ok(mappedValues);
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
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            var newCategory = _mapper.Map<Category>(dto);

            // İŞTE BURASI ÖNEMLİ: Artık aşçının güncel metodunu (AddCategoryAsync) çağırıyoruz!
            await _categoryService.AddCategoryAsync(newCategory);

            return Ok("Kategori başarıyla eklendi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto dto)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(dto.CategoryId);
            if (existingCategory == null) return NotFound("Kategori bulunamadı!");

            _mapper.Map(dto, existingCategory); // Yeni bilgileri eskisinin üzerine yaz

            // İŞTE ÇÖZÜM BURADA: await kelimesini ekledik!
            await _categoryService.UpdateCategoryAsync(existingCategory);

            return Ok("Kategori başarıyla güncellendi!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok("Kategori başarıyla silindi.");
        }
    }
}
