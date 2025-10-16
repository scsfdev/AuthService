
namespace AuthService.Application.Dtos
{
    public class AppUserRegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
