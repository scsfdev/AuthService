using AuthService.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data
{
    public class AuthDbContext(DbContextOptions options) : IdentityDbContext<IdentityAppUser>(options)
    {
    }
}
