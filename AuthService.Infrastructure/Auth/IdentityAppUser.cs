using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Auth
{
    public class IdentityAppUser: IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
