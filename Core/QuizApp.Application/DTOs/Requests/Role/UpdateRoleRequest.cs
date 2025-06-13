namespace QuizApp.Application.DTOs.Requests.Role;

public class UpdateRoleRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<string> Claims { get; set; } = new();
}