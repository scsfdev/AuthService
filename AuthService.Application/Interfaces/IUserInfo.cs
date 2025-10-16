
namespace AuthService.Application.Interfaces
{
    public interface IUserInfo
    {
        string Id { get; }
        string Email { get; }
        IList<string> Roles { get; }
    }
}
