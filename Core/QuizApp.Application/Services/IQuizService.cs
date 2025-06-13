using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;

namespace QuizApp.Application.Services;

public interface IQuizService
{
    Task<QuizResponse> CreateAsync(CreateQuizRequest request);
    Task<QuizResponse> UpdateAsync(UpdateQuizRequest request);
    Task DeleteAsync(DeleteQuizRequest request);
    Task<QuizResponse> GetByIdAsync(GetQuizByIdRequest request);
    Task<IEnumerable<QuizResponse>> GetAllAsync(GetQuizzesRequest request);
    Task<IEnumerable<QuizResponse>> GetByCategoryAsync(GetQuizzesByCategoryRequest request);
    Task<IEnumerable<QuizResponse>> GetByUserAsync(GetQuizzesByUserRequest request);
    Task<IEnumerable<QuizResponse>> GetActiveAsync(GetActiveQuizzesRequest request);
    Task<IEnumerable<QuizResponse>> CreateRangeAsync(CreateRangeQuizRequest request);
    Task<IEnumerable<QuizResponse>> UpdateRangeAsync(UpdateRangeQuizRequest request);
    Task DeleteRangeAsync(DeleteRangeQuizRequest request);
}