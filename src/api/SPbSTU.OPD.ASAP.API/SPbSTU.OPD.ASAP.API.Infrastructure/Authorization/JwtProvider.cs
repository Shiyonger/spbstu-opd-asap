using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Infrastucture.Settings;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Authorization;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken(Domain.Models.User user)
    {
        Claim[] claims =
        [
            new("userName", user.Name),
            new("login", user.Login),
            new("githubLink", user.GithubLink),
            new("role", Enum.GetName(user.Role) ?? string.Empty),
        ];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
        expires:
        DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}