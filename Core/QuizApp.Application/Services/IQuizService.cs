using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.Services;

public interface IQuizService
{
    Task<QuizDetailResponse> GetByIdAsync(GetQuizByIdRequest request);
    List<QuizDetailResponse> GetAll(GetQuizzesRequest request);
    Task<QuizDetailResponse> CreateAsync(CreateQuizRequest request);
    QuizDetailResponse Update(UpdateQuizRequest request);
    Task DeleteAsync(DeleteQuizRequest request);
    bool DeleteRange(DeleteRangeQuizRequest request);
} 