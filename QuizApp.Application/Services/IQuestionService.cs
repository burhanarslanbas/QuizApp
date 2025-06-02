using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.Services;

public interface IQuestionService
{
    Task<QuestionDetailResponse> GetByIdAsync(GetQuestionByIdRequest request);
    List<QuestionDetailResponse> GetAll(GetQuestionsRequest request);
    Task<QuestionDetailResponse> CreateAsync(CreateQuestionRequest request, Guid userId);
    Task<QuestionDetailResponse> Update(UpdateQuestionRequest request, Guid userId);
    Task DeleteAsync(DeleteQuestionRequest request, Guid userId);
    Task<bool> DeleteRange(DeleteRangeQuestionRequest request, Guid userId);
} 