using ECommerce.API.DTOs.ProductDtos;
using ECommerce.API.Entities;
using ECommerce.API.Interfaces;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ECommerce.API.Controllers
{
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

        // 🟢 KİLİT YOK: Herkes ürünleri görebilir (AllowAnonymous yazmaya gerek yok)
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var values = await _productService.GetProductsWithCategoryAsync();
            var mappedValues = _mapper.Map<List<ResultProductDto>>(values);
            return Ok(mappedValues);
        }

        // 🟢 KİLİT YOK: Herkes ürün detayına bakabilir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Bu ID'ye ait ürün bulunamadı.");

            var mappedProduct = _mapper.Map<ResultProductDto>(product);
            return Ok(mappedProduct);
        }

        // 🛑 KİLİTLİ: SADECE ADMİN ÜRÜN EKLEYEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            var newProduct = _mapper.Map<Product>(dto);
            await _productService.AddProductAsync(newProduct);
            return Ok("Ürün başarıyla eklendi.");
        }

        // 🛑 KİLİTLİ: SADECE ADMİN RESİM YÜKLEYEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // ... resim yükleme kodların aynı kalıyor ...
            if (file == null || file.Length == 0) return BadRequest("Lütfen geçerli bir resim dosyası seçin.");
            var extension = Path.GetExtension(file.FileName);
            var newImageName = Guid.NewGuid() + extension;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newImageName);
            using (var stream = new FileStream(location, FileMode.Create)) { await file.CopyToAsync(stream); }

            return Ok(new { Message = "Resim başarıyla yüklendi!", ImageUrl = "/images/" + newImageName });
        }

        // 🛑 KİLİTLİ: SADECE ADMİN GÜNCELLEYEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto dto)
        {
            var existingProduct = await _productService.GetProductByIdAsync(dto.ProductId);
            if (existingProduct == null) return NotFound("Ürün bulunamadı!");

            _mapper.Map(dto, existingProduct);
            await _productService.UpdateProductAsync(existingProduct);
            return Ok("Ürün başarıyla güncellendi.");
        }

        // 🛑 KİLİTLİ: SADECE ADMİN SİLEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Ürün başarıyla silindi.");
        }
    }
}