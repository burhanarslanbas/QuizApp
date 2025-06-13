using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;

namespace QuizApp.Application.Services;

public interface IQuestionService
{
    Task<QuestionResponse> CreateAsync(CreateQuestionRequest request);
    Task<QuestionResponse> UpdateAsync(UpdateQuestionRequest request);
    Task DeleteAsync(DeleteQuestionRequest request);
    Task<QuestionResponse> GetByIdAsync(GetQuestionByIdRequest request);
    Task<IEnumerable<QuestionResponse>> GetAllAsync(GetQuestionsRequest request);
    Task<IEnumerable<QuestionResponse>> GetByRepoAsync(GetQuestionsByRepoRequest request);
    Task<IEnumerable<QuestionResponse>> GetByCategoryAsync(GetQuestionsByCategoryRequest request);
    Task<IEnumerable<QuestionResponse>> CreateRangeAsync(CreateRangeQuestionRequest request);
    Task<IEnumerable<QuestionResponse>> UpdateRangeAsync(UpdateRangeQuestionRequest request);
    Task DeleteRangeAsync(DeleteRangeQuestionRequest request);
    Task UpdateRepoIdAsync(UpdateQuestionRepoIdRequest request);
    Task UpdateRepoIdsAsync(UpdateQuestionRepoIdsRequest request);
}