namespace ECommerce.API.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }



        // İlişki (Bire-Çok): Bir kullanıcının birden fazla siparişi olabilir
        public ICollection<Order> Orders { get; set; } = null!;
    }
}
