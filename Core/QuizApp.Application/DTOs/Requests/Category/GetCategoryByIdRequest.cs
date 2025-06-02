namespace QuizApp.Application.DTOs.Requests.Category;

public record GetCategoryByIdRequest
{
    public Guid Id { get; set; }
} 