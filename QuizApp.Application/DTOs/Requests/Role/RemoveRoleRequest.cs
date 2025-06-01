namespace QuizApp.Application.DTOs.Requests.Role;

public class RemoveRoleRequest
{
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
} 