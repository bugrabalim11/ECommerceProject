using ECommerce.API.Context;
using ECommerce.API.Interfaces;
using ECommerce.API.Entities;
using Microsoft.EntityFrameworkCore; // FirstOrDefaultAsync kullanabilmek için bu şart

namespace ECommerce.API.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ECommerceContext context) : base(context)
        {
        }

        // İŞTE SÖZLEŞMEYE EKLEDİĞİMİZ O YENİ METOT BURADA!
        public async Task<User?> GetByEmailAsync(string email)
        {
            // Veritabanına git, Users tablosuna bak ve emaili uyuşan ilk kişiyi getir
            return await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == email);
        }
    }
}
