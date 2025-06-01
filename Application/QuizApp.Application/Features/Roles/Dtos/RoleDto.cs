namespace QuizApp.Application.Features.Roles.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public List<ClaimDto> Claims { get; set; } = new();
} 