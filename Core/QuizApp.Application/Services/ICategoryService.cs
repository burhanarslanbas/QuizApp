using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;

namespace QuizApp.Application.Services;

public interface ICategoryService
{
    Task<CategoryDetailResponse> GetByIdAsync(GetCategoryByIdRequest request);
    List<CategoryDetailResponse> GetAll(GetCategoriesRequest request);
    Task<CategoryDetailResponse> CreateAsync(CreateCategoryRequest request);
    Task<List<CategoryDetailResponse>> CreateRange(CreateRangeCategoryRequest request);
    CategoryDetailResponse Update(UpdateCategoryRequest request);
    List<CategoryDetailResponse> UpdateRange(UpdateRangeCategoryRequest request);
    Task DeleteAsync(DeleteCategoryRequest request);
    bool DeleteRange(DeleteRangeCategoryRequest request);
}
