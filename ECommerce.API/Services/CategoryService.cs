using ECommerce.API.Entities;
using ECommerce.API.Interfaces;


namespace ECommerce.API.Services
{
    public class CategoryService : ICategoryService
    {
        // Aşçımız (Service), Depocuyu (Repository) tanımak zorunda!
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            // Önce depocuya "Git bu ID'ye sahip kategoriyi bul" diyoruz
            var category = await _categoryRepository.GetByIdAsync(id);

            // Eğer bulduysa, "Şimdi bunu sil" diyoruz.
            if (category != null)
            {
                _categoryRepository.Delete(category);
            }

        }

        // 1. Metodun isminin yanına 'async' kelimesini ekliyoruz
        public async Task AddCategoryAsync(Category category)
        {
            // 2. O hata fırlatan satırı SİLİYORUZ ve yerine Depocuyu (Repository) çağırıyoruz:
            await _categoryRepository.AddAsync(category);
        }
    }
}
