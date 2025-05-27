using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;

namespace QuizApp.Application.Services;

public interface IQuizResultService
{
    Task<bool> CreateAsync(CreateQuizResultRequest request);
    Task<List<QuizResultDTO>> CreateRangeAsync(List<CreateQuizResultRequest> requests);
    Task<bool> DeleteAsync(Guid id);
    bool DeleteRange(List<Guid> ids);
    List<QuizResultDTO> GetAll();
    Task<QuizResultDTO> GetByIdAsync(Guid id);
    bool Update(UpdateQuizResultRequest request);
} 