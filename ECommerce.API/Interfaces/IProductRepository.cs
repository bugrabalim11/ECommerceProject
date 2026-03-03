using ECommerce.API.Entities;


namespace ECommerce.API.Interfaces
{
    // Ürün depomuz, Genel depomuzun (IGenericRepository) tüm yeteneklerine (Ekle, Sil, Getir) bedavadan sahip oluyor!
    public interface IProductRepository : IGenericRepository<Product>
    {
        // İleride "Sadece fiyatı 500'den büyük olan ürünleri getir" gibi 
        // Ürüne özel SQL işlemleri olursa buraya yazacağız. Şimdilik içi boş.
    }
}
