using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Application.Services.Token;
using QuizApp.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizApp.Infrastructure.Managers.Token;

public class TokenManager : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly Application.Options.TokenOptions _tokenOptions;
    private const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

    public TokenManager(
        UserManager<AppUser> userManager,
        ITokenBlacklistService tokenBlacklistService,
        IOptions<Application.Options.TokenOptions> tokenOptions)
    {
        _userManager = userManager;
        _tokenBlacklistService = tokenBlacklistService;
        _tokenOptions = tokenOptions.Value;
    }

    public QuizApp.Application.DTOs.Responses.Token.Token CreateAccessToken(int expirationMinutes, AppUser user)
    {
        var roles = _userManager.GetRolesAsync(user).Result;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(RoleClaimType, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var refreshToken = Guid.NewGuid().ToString();

        return new QuizApp.Application.DTOs.Responses.Token.Token
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _tokenOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = _tokenOptions.Audience,
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(request.AccessToken, tokenValidationParameters, out var validatedToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new LoginErrorResponse { Message = "Invalid token" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new LoginErrorResponse { Message = "User not found" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newToken = CreateAccessToken(30, user);

            return new LoginSuccessResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                Token = newToken
            };
        }
        catch (Exception)
        {
            return new LoginErrorResponse { Message = "Invalid token" };
        }
    }

    public async Task<LoginResponse> RevokeTokenAsync(RevokeTokenRequest request)
    {
        await _tokenBlacklistService.BlacklistTokenAsync(request.RefreshToken);
        return new LoginSuccessResponse
        {
            Success = true,
            Message = "Token revoked successfully"
        };
    }

    public string CreateToken(AppUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(RoleClaimType, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)),
            ValidateLifetime = false,
            ValidIssuer = _tokenOptions.Issuer,
            ValidAudience = _tokenOptions.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }
}