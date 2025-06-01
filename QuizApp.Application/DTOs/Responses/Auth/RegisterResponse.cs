namespace QuizApp.Application.DTOs.Responses.Auth;

public class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class RegisterSuccessResponse : RegisterResponse
{
    public UserInfo UserInfo { get; set; } = new();
}

public class RegisterErrorResponse : RegisterResponse
{
    public List<string> Errors { get; set; } = new();
}