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
    private readonly Application.Options.TokenOptions _tokenOptions;
    private const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private const string RefreshTokenProvider = "RefreshTokenProvider";
    private const string RefreshTokenPurpose = "RefreshToken";

    public TokenManager(
        UserManager<AppUser> userManager,
        IOptions<Application.Options.TokenOptions> tokenOptions)
    {
        _userManager = userManager;
        _tokenOptions = tokenOptions.Value;
    }

    public async Task<Application.DTOs.Responses.Token.Token> CreateAccessTokenAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
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

        // Identity'nin token yapısını kullanarak refresh token oluştur
        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user,
            RefreshTokenProvider,
            RefreshTokenPurpose
        );

        // Refresh token'ı kullanıcıya kaydet
        await _userManager.SetAuthenticationTokenAsync(
            user,
            RefreshTokenProvider,
            RefreshTokenPurpose,
            refreshToken
        );

        return new Application.DTOs.Responses.Token.Token
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenExpiration)
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return new LoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid access token",
                    Errors = new List<string> { "Invalid access token" }
                };
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new LoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid access token",
                    Errors = new List<string> { "Invalid access token" }
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new LoginErrorResponse
                {
                    Success = false,
                    Message = "User not found",
                    Errors = new List<string> { "User not found" }
                };
            }

            // Identity'nin token doğrulama mekanizmasını kullan
            var isValid = await _userManager.VerifyUserTokenAsync(
                user,
                RefreshTokenProvider,
                RefreshTokenPurpose,
                request.RefreshToken
            );

            if (!isValid)
            {
                return new LoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid refresh token",
                    Errors = new List<string> { "Invalid refresh token" }
                };
            }

            var newToken = await CreateAccessTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList();

            var userDto = new Application.DTOs.Responses.User.UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles.ToList(),
                Claims = claims
            };

            return new LoginSuccessResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                Token = newToken,
                User = userDto
            };
        }
        catch (Exception ex)
        {
            return new LoginErrorResponse
            {
                Success = false,
                Message = "Token refresh failed",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<LoginResponse> RevokeTokenAsync(RevokeTokenRequest request)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return new LoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid token",
                    Errors = new List<string> { "Invalid token" }
                };
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new LoginErrorResponse
                {
                    Success = false,
                    Message = "Invalid token",
                    Errors = new List<string> { "Invalid token" }
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Identity'nin token yapısından refresh token'ı kaldır
                await _userManager.RemoveAuthenticationTokenAsync(
                    user,
                    RefreshTokenProvider,
                    RefreshTokenPurpose
                );
            }

            return new LoginSuccessResponse
            {
                Success = true,
                Message = "Token revoked successfully"
            };
        }
        catch (Exception ex)
        {
            return new LoginErrorResponse
            {
                Success = false,
                Message = "Token revocation failed",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
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
        catch
        {
            return null;
        }
    }
}