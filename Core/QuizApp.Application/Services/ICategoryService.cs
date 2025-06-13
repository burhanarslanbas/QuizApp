using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;

namespace QuizApp.Application.Services;

public interface ICategoryService
{
    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
    Task<CategoryResponse> UpdateAsync(UpdateCategoryRequest request);
    Task DeleteAsync(DeleteCategoryRequest request);
    Task<CategoryResponse> GetByIdAsync(GetCategoryByIdRequest request);
    Task<IEnumerable<CategoryResponse>> GetAllAsync(GetCategoriesRequest request);
    Task<IEnumerable<CategoryResponse>> CreateRangeAsync(CreateRangeCategoryRequest request);
    Task<IEnumerable<CategoryResponse>> UpdateRangeAsync(UpdateRangeCategoryRequest request);
    Task DeleteRangeAsync(DeleteRangeCategoryRequest request);
}
