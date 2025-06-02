namespace QuizApp.Application.DTOs.Requests.Auth;

public class LoginRequest
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
}
