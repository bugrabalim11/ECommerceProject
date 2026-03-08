using ECommerce.API.DTOs.ProductDtos;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ECommerce.API.Controllers
{
    [Authorize] // Güvenlik kapımız kapalı!
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var values = await _productService.GetProductsWithCategoryAsync();
            var mappedValues = _mapper.Map<List<ResultProductDto>>(values);
            return Ok(mappedValues);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Bu ID'ye ait ürün bulunamadı.");

            // TEK BİR ÜRÜNÜ DTO'YA ÇEVİRİYORUZ
            var mappedProduct = _mapper.Map<ResultProductDto>(product);
            return Ok(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto) // HAM PRODUCT YERİNE DTO GELDİ!
        {
            var newProduct = _mapper.Map<Product>(dto);
            await _productService.AddProductAsync(newProduct);
            return Ok("Ürün başarıyla eklendi.");
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // 1. Gelen bir dosya var mı kontrol et
            if (file == null || file.Length == 0)
            {
                return BadRequest("Lütfen geçerli bir resim dosyası seçin.");
            }

            // 2. Dosyanın uzantısını al (örn: .jpg, .png)
            var extension = Path.GetExtension(file.FileName);

            // 3. Resme benzersiz bir isim ver (Aynı isimde iki resim çakışmasın)
            var newImageName = Guid.NewGuid() + extension;

            // 4. Resmin kaydedileceği tam yolu belirle (wwwroot/images klasörü)
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newImageName);

            // 5. Resmi fiziksel olarak o klasöre kopyala
            using (var stream = new FileStream(location, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 6. Başarılı olursa resmin yeni adını geri dön!
            return Ok(new { Message = "Resim başarıyla yüklendi!", ImageUrl = "/images/" + newImageName });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto dto) // HAM PRODUCT YERİNE DTO GELDİ!
        {
            var existingProduct = await _productService.GetProductByIdAsync(dto.ProductId);
            if (existingProduct == null) return NotFound("Ürün bulunamadı!");

            _mapper.Map(dto, existingProduct); // Yeni bilgileri eskisinin üzerine yaz!

            await _productService.UpdateProductAsync(existingProduct);
            return Ok("Ürün başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Ürün başarıyla silindi.");
        }
    }
}