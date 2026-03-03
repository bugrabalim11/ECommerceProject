using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Interfaces
{
    // <T> harfi "Joker" demektir. Yerine Category de gelebilir, Product da!
    public interface IGenericRepository<T> where T : class
    {

        // İşte burası! Service içinden "Include" gibi karmaşık sorgular yapabilmemizi sağlar.
        IQueryable<T> Table { get; }


        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Profesyonel projelerde veritabanı işlemleri sistemi kilitlemesin diye asenkron yapılır.
    }
}
 
