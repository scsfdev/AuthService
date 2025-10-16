using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using Shared.Events;

namespace AuthService.Application.Services
{
    public class AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtGenerator, 
        IEventPublisher publisher) : IAuthService
    {
        public async Task<string> ChangePasswordAsync(AppUserChangePwdDto changePwdRequestDto)
        {
            return await userRepository.ChangePasswordAsync(changePwdRequestDto.Email, changePwdRequestDto.Password, changePwdRequestDto.NewPassword);
        }

        public async Task<string> DeactivateAsync(Guid userGuid)
        {
            var user = await userRepository.GetUserByGuidAsync(userGuid.ToString());
            if (user == null)
                return "User not found!";

            var result = await userRepository.DeactivateUser(user);
            // publish user registered event
            var userEvent = new UserDeactivatedEvent
            {
                UserId = user.Id,
                Email = user.Email
            };

            await publisher.PublishAsync(userEvent);

            return result;
        }

        public async Task<AuthResponseDto?> LoginAsync(AppUserLoginDto loginRequestDto)
        {
            var user = await userRepository.GetUserByEmailAsync(loginRequestDto.Email);
            if (user == null || !await userRepository.CheckPasswordAsync(user, loginRequestDto.Password))
                return null;

            return jwtGenerator.GenerateToken(user);
        }

        public async Task<AuthResponseDto?> RegisterAsync(AppUserRegisterDto registerRequestDto)
        {
            var user = await userRepository.RegisterUserAsync(registerRequestDto.Email, registerRequestDto.Password);
            if (user == null)
                return null;

            // publish user registered event
            var userEvent = new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email,
                DisplayName = registerRequestDto.DisplayName
            };

            await publisher.PublishAsync(userEvent);

            return jwtGenerator.GenerateToken(user);
        }
    }
}
