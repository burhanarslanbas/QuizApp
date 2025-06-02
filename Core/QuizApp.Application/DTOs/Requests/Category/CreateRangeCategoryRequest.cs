using QuizApp.Application.DTOs.Requests.Category;

namespace QuizApp.Application.DTOs.Requests.Category
{
    public record CreateRangeCategoryRequest
    {
        public List<CreateCategoryRequest> Categories { get; set; }
    }
}
