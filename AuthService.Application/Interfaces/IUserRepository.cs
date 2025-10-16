
namespace AuthService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IUserInfo?> GetUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(IUserInfo user, string password);
        Task<string> ChangePasswordAsync(string userEmail, string oldPassword, string newPassword);
        Task<string> DeactivateUser(IUserInfo user);
        Task<IUserInfo?> RegisterUserAsync(string email, string password);
        
        Task<IUserInfo?> GetUserByGuidAsync(string userGuid);
    }
}
