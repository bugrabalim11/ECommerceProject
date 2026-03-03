using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories
{
    // "Ben IGenericRepository sözleşmesindeki tüm kurallara uyacağım" diyoruz.
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ECommerceContext _context;
        private readonly DbSet<T> _dbSet;

        // Dependency Injection ile veritabanı köprümüzü (Context) içeri alıyoruz
        public GenericRepository(ECommerceContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // Joker tabloyu ayarlıyoruz
        }

        public IQueryable<T> Table => _context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}