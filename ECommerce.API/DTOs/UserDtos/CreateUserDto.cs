namespace ECommerce.API.DTOs.UserDtos
{
    public class CreateUserDto
    {
        // UserId ve CreatedAt yok! Onları sistem otomatik verecek.
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
    }
}
