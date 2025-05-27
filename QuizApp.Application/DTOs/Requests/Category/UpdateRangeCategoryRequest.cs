namespace QuizApp.Application.DTOs.Requests.Category;

public record UpdateRangeCategoryRequest
{
    public List<Guid> Ids { get; set; }
    public List<UpdateCategoryRequest> Categories { get; set; }
}
