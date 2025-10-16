
using AuthService.Application.Dtos;

namespace AuthService.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        AuthResponseDto GenerateToken(IUserInfo user);
    }
}
