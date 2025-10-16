using AuthService.Application.Dtos;


namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(AppUserLoginDto loginRequestDto);
        Task<AuthResponseDto?> RegisterAsync(AppUserRegisterDto registerRequestDto);
        Task<string> ChangePasswordAsync(AppUserChangePwdDto changePwdRequestDto);
        Task<string> DeactivateAsync(Guid userGuid);
    }
}
