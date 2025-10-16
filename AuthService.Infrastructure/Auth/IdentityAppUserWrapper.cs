using AuthService.Application.Interfaces;

namespace AuthService.Infrastructure.Auth
{
    public class IdentityAppUserWrapper(IdentityAppUser user, IList<string> roles) : IUserInfo
    {
        public string Id { get; } = user.Id;

        public string Email { get; } = user.Email!;

        public IList<string> Roles { get; } = roles;
    }
}
