using ECommerce.API.Entities;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.API.Context
{
    // DbContext sınıfından miras alarak bu sınıfı bir veritabanı köprüsü yapıyoruz
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {

        }


        // C# Sınıfı (Sol) <---> SQL Tablosu (Sağ) Eşleştirmeleri
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
        
    }
}
