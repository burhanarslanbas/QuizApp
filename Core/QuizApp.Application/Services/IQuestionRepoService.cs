using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;

namespace QuizApp.Application.Services;

public interface IQuestionRepoService
{
    Task<QuestionRepoResponse> CreateAsync(CreateQuestionRepoRequest request);
    Task<QuestionRepoResponse> UpdateAsync(UpdateQuestionRepoRequest request);
    Task DeleteAsync(DeleteQuestionRepoRequest request);
    Task<QuestionRepoResponse> GetByIdAsync(GetQuestionRepoByIdRequest request);
    Task<IEnumerable<QuestionRepoResponse>> GetAllAsync(GetQuestionReposRequest request);
    Task<IEnumerable<QuestionRepoResponse>> GetByUserAsync(GetQuestionReposByUserRequest request);
    Task DeleteRangeAsync(DeleteRangeQuestionRepoRequest request);
}