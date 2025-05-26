namespace QuizApp.Application.DTOs.Requests.Category.Write
{
    public record UpdateCategoryRequest(Guid Id, string Name, string Description);
}
