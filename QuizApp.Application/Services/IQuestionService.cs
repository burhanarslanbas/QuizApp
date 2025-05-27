using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.Services;

public interface IQuestionService
{
    Task<QuestionDetailResponse> GetByIdAsync(GetQuestionByIdRequest request);
    List<QuestionDetailResponse> GetAll(GetQuestionsRequest request);
    Task<QuestionDetailResponse> CreateAsync(CreateQuestionRequest request);
    QuestionDetailResponse Update(UpdateQuestionRequest request);
    Task DeleteAsync(DeleteQuestionRequest request);
    bool DeleteRange(DeleteRangeQuestionRequest request);
} 