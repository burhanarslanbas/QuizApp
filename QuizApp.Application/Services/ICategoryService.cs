using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;

namespace QuizApp.Application.Services;

public interface ICategoryService
{
    Task<bool> CreateAsync(CreateCategoryRequest request);
    Task<List<CategoryDTO>> CreateRangeAsync(List<CreateCategoryRequest> requests);
    Task<CategoryDTO> Update(UpdateCategoryRequest request);
    // Task<List<CategoryDTO>> UpdateRange(List<UpdateCategoryRequest> requests);
    Task<bool> Delete(Guid id);
    Task<bool> DeleteRange(List<Guid> ids);
    Task<CategoryDTO> GetById(Guid id);
    Task<List<CategoryDTO>> GetAll();
}
