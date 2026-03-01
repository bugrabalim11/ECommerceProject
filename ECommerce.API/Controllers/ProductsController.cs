
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.ProductDtos;
using Microsoft.EntityFrameworkCore;
using FluentValidation;


namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
      private readonly ECommerceContext _context;
        public ProductsController(ECommerceContext context)
        {
            _context = context;
        }


        [HttpGet]   // Ürünleri listeleme
        public IActionResult GetProductList()
        {
            var values =_context.Products
                .Include(x=>x.Category)  // Kategoriyi dahil et
                .Select(y=> new ResultProductDto   // Kategoriyi dahil et
                {
                    ProductId = y.ProductId,
                    ProductName = y.ProductName,
                    ProductPrice = y.ProductPrice,
                    CategoryName = y.Category.CategoryName   // İşte sihir burada! Kategori nesnesinin içinden sadece adını cımbızladık.
                })
                .ToList();

            return Ok(values);
        }

        [HttpPost]   // Ürün ekleme  
        public IActionResult CreateProduct(CreateProductDto dto, [FromServices] IValidator<CreateProductDto> validator)
        {
            // 1. KAPI KONTROLÜ: Gelen veriyi (dto) güvenlik görevlisine ver, kurallara uyuyor mu baksın.
            var validationResult = validator.Validate(dto);

            // 2. EĞER KURALLARA UYMUYORSA: (Örn: Fiyat eksi ise veya isim boşsa)
            if (!validationResult.IsValid)
            {
                // İçeri girmesine izin verme! Hata mesajlarını (400 Bad Request) kullanıcının yüzüne çarp.
                return BadRequest(validationResult.Errors);
            }


            // Dışarıdan gelen DTO'yu, veritabanına kaydedilecek Entity'ye çeviriyoruz
            Product product =new Product
            {
                CategoryId = dto.CategoryId,
                ProductName = dto.ProductName,
                ProductDescription = dto.ProductDescription,
                ProductPrice = dto.ProductPrice,
                StockQuantity = dto.StockQuantity
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok("Ürün Başarıyla Eklendi!");
        }

        [HttpDelete("{id}")]   // Ürün silme
        public IActionResult DeleteProduct(int id)
        {
            var value = _context.Products.Find(id);
            if ( value== null)
            {
                return NotFound("Silinnecek Ürün bulunamadı!");
            }

            _context.Products.Remove(value);
            _context.SaveChanges();
            return Ok("Ürün Başarıyla Silindi!");
        }


        [HttpPut]  //Üürn Güncelleme
        public IActionResult UpdateProduct(UpdateProductDto dto)
        {
            // 1. DTO'dan gelen ProductId ile veritabanında o ürünü bul
            var value = _context.Products.Find(dto.ProductId);
            if (value== null)
            {
                return NotFound("Güncellenecek Ürün bulunamadı!");
            }

            value.CategoryId = dto.CategoryId;
            value.ProductName = dto.ProductName;
            value.ProductDescription = dto.ProductDescription;
            value.ProductPrice = dto.ProductPrice;
            value.StockQuantity = dto.StockQuantity;

            _context.SaveChanges();
            return Ok("Ürün Başarıyla Güncellendi!");
        }
    }
}
