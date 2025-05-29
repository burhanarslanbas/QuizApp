namespace QuizApp.Application.DTOs.Responses.Auth;

public class LoginResponse
{
}

public class LoginSuccessResponse : LoginResponse
{
    public Token.Token Token { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
public class LoginErrorResponse : LoginResponse
{
    public string ErrorMessage { get; set; }
    public int StatusCode { get; set; }
    public string? Details { get; set; }
}
