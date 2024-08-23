using Domain.Interfaces.Services;
using Domain.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Domain.Utils.Useful;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        public AuthService() { }
        public string GenerateJwtToken(string email, string permission, Guid id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);

            var claims = new[]
            {
                new Claim(CustomClaimTypes.Name, email),
                new Claim(CustomClaimTypes.Permission, permission),
                new Claim(CustomClaimTypes.NameIdentifier, id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.ToUniversalTime().AddMinutes(Useful.TOKEN_JWT_EXPIRES_IN_30_MIN),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
