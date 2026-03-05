namespace ECommerce.API.DTOs.UserDtos
{
    public class ResultUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Şifre alanı yok! Sırrımız güvende.
    }
}
