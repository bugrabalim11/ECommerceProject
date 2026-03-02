using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.CategoryDtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 1. BÜTÜN CONTROLLER KİLİTLENDİ! (Kimliksiz kimse giremez)
    public class CategoriesController : ControllerBase
    {
        // 1. Senin oluşturduğun veritabanı sınıfını tanımlıyoruz
        private readonly ECommerceContext _context;
        
        // 2. Garsona senin mutfağının (ECommerceContext) anahtarını veriyoruz
        public CategoriesController(ECommerceContext context)
        {
            _context = context;
        }


        [AllowAnonymous]  // Tekil getirme de vitrindir, o da açık kalsın
        [HttpGet]
        public IActionResult GetCategoryList()
        {
            var values = _context.Categories.ToList();
            return Ok(values);
        }


        [HttpPost]   // Veri ekleme işlemleri için her zaman HttpPost kullanırız
        public IActionResult CreateCategory(CreateCategoryDto dto , [FromServices] IValidator<CreateCategoryDto> validator) 
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            Category newCategory = new Category
            {
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return Ok("Kategori Başarıyla Eklendi!");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var values = _context.Categories.Find(id);
            if (values == null)
            {
                return NotFound("Kategori bulunamadı!");
            }
            _context.Categories.Remove(values);
            _context.SaveChanges();
            return Ok("Kategori başarıyla silindi!");
        }


        [HttpPut]
        public IActionResult UpdateCategoryDto(UpdateCategoryDto dto)
        {
            var values = _context.Categories.Find(dto.CategoryId);  // Artık Entity değil, DTO alıyoruz!

            if (values == null)
            {
                return NotFound("Kategori bulunamadı!");
            }


            // 2. DTO'dan gelen yeni değerleri, veritabanındaki eski değerlerin üzerine yazıyoruz
            values.CategoryName = dto.CategoryName;
            values.CategoryDescription = dto.CategoryDescription;

            _context.SaveChanges();

            return Ok("Kategori Başarıyla Güncellendi!");
        }

    }
}
