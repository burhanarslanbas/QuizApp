namespace QuizApp.Application.DTOs.Requests.Category;

public record UpdateCategoryRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
