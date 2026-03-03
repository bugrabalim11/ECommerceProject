using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;

namespace ECommerce.API.Repositories
{
    // "Ben bir Ürün Depocusuyum, Genel Depo kurallarına ve Ürün Sözleşmesine uyarım" diyoruz.
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        // Veritabanı köprümüzü (Context) alıp üst sınıfa gönderiyoruz
        public ProductRepository(ECommerceContext context) : base(context)
        {
        }
    }
}
