namespace QuizApp.Application.DTOs.Requests.Role;

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Claims { get; set; } = new();
}