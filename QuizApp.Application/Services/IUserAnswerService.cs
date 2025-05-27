using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.Services;

public interface IUserAnswerService
{
    Task<UserAnswerDetailResponse> GetByIdAsync(GetUserAnswerByIdRequest request);
    List<UserAnswerDetailResponse> GetAll(GetUserAnswersRequest request);
    List<UserAnswerDetailResponse> GetByQuizResult(GetUserAnswersByQuizResultRequest request);
    Task<UserAnswerDetailResponse> CreateAsync(CreateUserAnswerRequest request);
    UserAnswerDetailResponse Update(UpdateUserAnswerRequest request);
    Task DeleteAsync(DeleteUserAnswerRequest request);
    bool DeleteRange(DeleteRangeUserAnswerRequest request);
} 