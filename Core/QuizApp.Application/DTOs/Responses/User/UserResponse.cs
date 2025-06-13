namespace QuizApp.Application.DTOs.Responses.User;

public record UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public List<string> Roles { get; set; } = new();
    public List<string> Claims { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}