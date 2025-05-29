namespace QuizApp.Application.Services.Token;

using QuizApp.Domain.Entities.Identity;

public interface ITokenHandler
{
    DTOs.Responses.Token.Token CreateAccessToken(int minute, AppUser user);
}
