using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Application.Services.Token
{
    public interface ITokenService
    {
        Task<QuizApp.Application.DTOs.Responses.Token.Token> CreateAccessTokenAsync(AppUser user);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<LoginResponse> RevokeTokenAsync(RevokeTokenRequest request);
    }
}