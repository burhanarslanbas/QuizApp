namespace QuizApp.Application.DTOs.Requests.Category;

public record CreateCategoryRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
