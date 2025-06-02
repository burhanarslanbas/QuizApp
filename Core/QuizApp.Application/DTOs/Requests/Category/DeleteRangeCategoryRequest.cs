namespace QuizApp.Application.DTOs.Requests.Category;

public record DeleteRangeCategoryRequest
{
    public List<Guid> Ids { get; set; }
}
