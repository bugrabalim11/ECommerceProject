using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;   // ToList() metodu için gerekli
using ECommerce.API.Context;
using ECommerce.API.Entities;
using ECommerce.API.DTOs.ProductDtos;


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
            var values = _context.Products.ToList();
            return Ok(values);
        }

        [HttpPost]   // Ürün ekleme  
        public IActionResult CreateProduct(CreateProductDto dto)
        {
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
