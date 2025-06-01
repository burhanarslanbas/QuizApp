using QuizApp.Application.DTOs.Responses.Token;

namespace QuizApp.Application.DTOs.Responses.Auth;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class LoginSuccessResponse : LoginResponse
{
    public QuizApp.Application.DTOs.Responses.Token.Token Token { get; set; } = new();
    public UserInfo UserInfo { get; set; } = new();
}

public class LoginErrorResponse : LoginResponse
{
    public List<string> Errors { get; set; } = new();
}

public class UserInfo
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
