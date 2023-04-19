using Documentally.Application.Interfaces.Infrastructure;
using Documentally.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Documentally.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> optionsJwtSettings)
        {
            _jwtSettings = optionsJwtSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpireDays));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.Value.ToString()),
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: expires,
                claims: claims,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
