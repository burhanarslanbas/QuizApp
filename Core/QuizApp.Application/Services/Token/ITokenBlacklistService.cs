namespace QuizApp.Application.Services.Token;

public interface ITokenBlacklistService
{
    Task BlacklistTokenAsync(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);
} 