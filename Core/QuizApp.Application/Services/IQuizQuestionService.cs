using QuizApp.Application.DTOs.Requests.QuizQuestion;
using QuizApp.Application.DTOs.Responses.QuizQuestion;

namespace QuizApp.Application.Services;

public interface IQuizQuestionService
{
    Task<QuizQuestionResponse> CreateAsync(CreateQuizQuestionRequest request);
    Task<QuizQuestionResponse> UpdateAsync(UpdateQuizQuestionRequest request);
    Task DeleteAsync(DeleteQuizQuestionRequest request);
    Task<QuizQuestionResponse> GetByIdAsync(GetQuizQuestionByIdRequest request);
    Task<IEnumerable<QuizQuestionResponse>> GetAllAsync(GetQuizQuestionsRequest request);
}