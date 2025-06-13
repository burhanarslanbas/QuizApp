using QuizApp.Application.DTOs.Responses.User;

namespace QuizApp.Application.DTOs.Responses.Auth;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class LoginSuccessResponse : LoginResponse
{
    public Token.Token Token { get; set; } = new();
    public UserResponse User { get; set; } = new();
}

public class LoginErrorResponse : LoginResponse
{
    public List<string> Errors { get; set; } = new();
}