using Microsoft.Extensions.Caching.Memory;
using QuizApp.Application.Services.Token;

namespace QuizApp.Infrastructure.Managers.Token;

public class TokenBlacklistManager : ITokenBlacklistService
{
    private readonly IMemoryCache _cache;
    private const string BlacklistPrefix = "blacklist_";

    public TokenBlacklistManager(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task BlacklistTokenAsync(string token)
    {
        var key = $"{BlacklistPrefix}{token}";
        _cache.Set(key, true, TimeSpan.FromDays(7)); // Token will be blacklisted for 7 days
        await Task.CompletedTask;
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        var key = $"{BlacklistPrefix}{token}";
        return await Task.FromResult(_cache.TryGetValue(key, out _));
    }
} 