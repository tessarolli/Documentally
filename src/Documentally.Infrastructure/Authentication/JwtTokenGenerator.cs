// <copyright file="JwtTokenGenerator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Documentally.Application.Abstractions.Authentication;
using Documentally.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Documentally.Infrastructure.Authentication;

/// <summary>
/// Jwt Token Generator.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings jwtSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
    /// </summary>
    /// <param name="optionsJwtSettings">JwtSetting injected.</param>
    public JwtTokenGenerator(IOptions<JwtSettings> optionsJwtSettings)
    {
        jwtSettings = optionsJwtSettings.Value;
    }

    /// <inheritdoc/>
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.ExpireDays));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.Value.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            expires: expires,
            claims: claims,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
