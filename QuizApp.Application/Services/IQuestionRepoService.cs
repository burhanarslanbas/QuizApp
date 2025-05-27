using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.Services;

public interface IQuestionRepoService
{
    Task<QuestionDetailResponse> GetByIdAsync(GetQuestionRepoByIdRequest request);
    List<QuestionDetailResponse> GetAll(GetQuestionReposRequest request);
    List<QuestionDetailResponse> GetByCategory(GetQuestionReposByCategoryRequest request);
    Task<QuestionDetailResponse> CreateAsync(CreateQuestionRepoRequest request);
    QuestionDetailResponse Update(UpdateQuestionRepoRequest request);
    Task DeleteAsync(DeleteQuestionRepoRequest request);
    bool DeleteRange(DeleteRangeQuestionRepoRequest request);
} 