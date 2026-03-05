using ECommerce.API.Entities;


namespace ECommerce.API.Interfaces
{
    // Ürün depomuz, Genel depomuzun (IGenericRepository) tüm yeteneklerine (Ekle, Sil, Getir) bedavadan sahip oluyor!
    public interface IProductRepository : IGenericRepository<Product>
    {
        // YENİ GÖREV: Ürünleri kategorileriyle birlikte getir.
        Task<List<Product>> GetProductsWithCategoryAsync();
    }
}
