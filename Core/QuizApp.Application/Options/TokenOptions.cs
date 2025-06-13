namespace QuizApp.Application.Options
{
    public class TokenOptions
    {
        public string SecurityKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpiration { get; set; } = 15; // 15 dakika
        public int RefreshTokenExpiration { get; set; } = 7; // 7 gün
    }
}