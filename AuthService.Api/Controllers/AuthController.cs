using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUserRegisterDto registerDto)
        {
            var authResponse = await authService.RegisterAsync(registerDto);
            if(authResponse == null)
                return BadRequest("Registration failed!");

            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLoginDto loginDto)
        {
            var authResponse = await authService.LoginAsync(loginDto);
            if(authResponse == null)
                return Unauthorized("Invalid email or password!");

            return Ok(authResponse);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] AppUserChangePwdDto changePwdDto)
        {
            var result = await authService.ChangePasswordAsync(changePwdDto);
            if(!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok("Password changed successfully.");
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> Deactivate([FromQuery] Guid userGuid)
        {
            var result = await authService.DeactivateAsync(userGuid);
            if(!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok("User deactivated successfully.");
        }
    }
}
