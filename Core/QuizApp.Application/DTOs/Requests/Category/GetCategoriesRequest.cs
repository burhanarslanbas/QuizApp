namespace QuizApp.Application.DTOs.Requests.Category;

public record GetCategoriesRequest
{
    public string? SearchText { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public bool? IsActive { get; set; } = true;
}