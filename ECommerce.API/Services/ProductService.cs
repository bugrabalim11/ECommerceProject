using ECommerce.API.Entities;
using ECommerce.API.Interfaces;

namespace ECommerce.API.Services
{
    public class ProductService : IProductService
    {
        // Aşçımız (Service), Ürün Depocusunu (Repository) tanımak zorunda!
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
        public async Task AddProductAsync(Product product)
        {
            // İleride "Aynı barkodlu ürün var mı?" kontrolünü buraya yazacağız.
            await _productRepository.AddAsync(product);
        }
        public async Task UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            // Önce depocuya "Git bu ID'ye sahip ürünü bul" diyoruz
            var product = await _productRepository.GetByIdAsync(id);

            // Eğer bulduysa, "Şimdi bunu sil" diyoruz.
            if (product!=null)
            {
                _productRepository.Delete(product);
            }
        }
    }
}
