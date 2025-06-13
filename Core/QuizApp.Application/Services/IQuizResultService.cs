using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;

namespace QuizApp.Application.Services;

public interface IQuizResultService
{
    Task<QuizResultResponse> CreateAsync(CreateQuizResultRequest request);
    Task<QuizResultResponse> UpdateAsync(UpdateQuizResultRequest request);
    Task DeleteAsync(DeleteQuizResultRequest request);
    Task<QuizResultResponse> GetByIdAsync(GetQuizResultByIdRequest request);
    Task<IEnumerable<QuizResultResponse>> GetAllAsync(GetQuizResultsRequest request);
    Task<IEnumerable<QuizResultResponse>> GetByUserAsync(GetQuizResultsByUserRequest request);
    Task<IEnumerable<QuizResultResponse>> GetByQuizAsync(GetQuizResultsByQuizRequest request);
    Task<IEnumerable<QuizResultResponse>> CreateRangeAsync(CreateRangeQuizResultRequest request);
    Task<IEnumerable<QuizResultResponse>> UpdateRangeAsync(UpdateRangeQuizResultRequest request);
    Task DeleteRangeAsync(DeleteRangeQuizResultRequest request);
}