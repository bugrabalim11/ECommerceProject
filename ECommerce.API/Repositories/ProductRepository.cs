using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using Microsoft.EntityFrameworkCore; // Include komutu için ŞART!

namespace ECommerce.API.Repositories
{
    // "Ben bir Ürün Depocusuyum, Genel Depo kurallarına ve Ürün Sözleşmesine uyarım" diyoruz.
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        // Veritabanı köprümüzü (Context) alıp üst sınıfa gönderiyoruz
        public ProductRepository(ECommerceContext context) : base(context)
        {
        }

        // İŞTE YENİ ÖZEL YETENEĞİMİZ!
        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            // Veritabanına "Ürünleri getir, gelirken Kategorilerini de DAHİL ET (Include)" diyoruz.
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }
    }
}
