using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Infrastructure.Auth
{
    public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
    {
        private readonly JwtOptions jwtOpts = jwtOptions.Value;
        
        public AuthResponseDto GenerateToken(IUserInfo user)
        {
            var claims = new List<Claim>
            {
                // JWT standard subject claim. Public subject claim, safe to expose.
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                // .NET identity standard email claim.
                new Claim(ClaimTypes.Email, user.Email),
                // NameIdentifier mapped to Id (Guid).
                new Claim(ClaimTypes.NameIdentifier, user.Id)
                //// JWT ID claim, unique for each token. Keep for token revocation future use.
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            
            // Add roles.
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Create the token.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOpts.Issuer,
                audience: jwtOpts.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOpts.ExpiryMins),
                signingCredentials: creds);

            var authresponse = new AuthResponseDto
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
            return authresponse;
        }

    }
}
