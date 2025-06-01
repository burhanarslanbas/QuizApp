namespace QuizApp.Application.Features.Roles.Dtos;

public class ClaimDto
{
    public int Id { get; set; }
    public string ClaimType { get; set; } = null!;
    public string ClaimValue { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
} 