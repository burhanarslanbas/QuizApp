using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;

namespace QuizApp.Application.Services;

public interface IUserAnswerService
{
    Task<UserAnswerResponse> CreateAsync(CreateUserAnswerRequest request);
    Task<UserAnswerResponse> UpdateAsync(UpdateUserAnswerRequest request);
    Task DeleteAsync(DeleteUserAnswerRequest request);
    Task<UserAnswerResponse> GetByIdAsync(GetUserAnswerByIdRequest request);
    Task<IEnumerable<UserAnswerResponse>> GetAllAsync(GetUserAnswersRequest request);
    Task<IEnumerable<UserAnswerResponse>> GetByQuizResultAsync(GetUserAnswersByQuizResultRequest request);
    Task DeleteRangeAsync(DeleteRangeUserAnswerRequest request);
}