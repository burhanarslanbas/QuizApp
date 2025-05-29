using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Application.DTOs.Responses.Token;
using QuizApp.Application.Services.Token;
using QuizApp.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizApp.Infrastructure.Handlers.Token;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public QuizApp.Application.DTOs.Responses.Token.Token CreateAccessToken(int minute, AppUser user)
    {
        var token = new QuizApp.Application.DTOs.Responses.Token.Token();

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        token.Expiration = DateTime.UtcNow.AddMinutes(minute);

        // Claim'leri ekle
        var claims = new List<Claim>();
        
        if (user != null)
        {
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });
        }

        JwtSecurityToken securityToken = new(
            issuer: _configuration["Token:Issuer"],
            audience: _configuration["Token:Audience"],
            claims: claims,
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );

        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);

        // Refresh Token oluştur
        token.RefreshToken = CreateRefreshToken();
        token.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7); // 7 günlük refresh token

        return token;
    }

    private string CreateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}
