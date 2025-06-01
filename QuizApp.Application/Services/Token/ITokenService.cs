using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Application.DTOs.Responses.Token;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Application.Services.Token
{
    public interface ITokenService
    {
        QuizApp.Application.DTOs.Responses.Token.Token CreateAccessToken(int expirationMinutes, AppUser user);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<LoginResponse> RevokeTokenAsync(RevokeTokenRequest request);
    }
} 