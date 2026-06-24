using Hospital.Application.Interfaces.Services;
using Hospital.Application.Setting;
using Hospital.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwt;

        public JwtService(IOptions<JwtSettings> jwtOption)
        {
            _jwt = jwtOption.Value;
        }

        public string GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,user.Id),
               new Claim(ClaimTypes.Email,user.Email?? string.Empty),
               new Claim(ClaimTypes.Name,user.FullName),
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: _jwt.Issuer,
             audience: _jwt.Audience,
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
             signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
