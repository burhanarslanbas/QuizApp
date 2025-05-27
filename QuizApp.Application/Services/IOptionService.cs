using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.Services;

public interface IOptionService
{
    Task<bool> CreateAsync(CreateOptionRequest request);
    Task<List<OptionDTO>> CreateRangeAsync(List<CreateOptionRequest> requests);
    Task<bool> DeleteAsync(Guid id);
    bool DeleteRange(List<Guid> ids);
    List<OptionDTO> GetAll();
    Task<OptionDTO> GetByIdAsync(Guid id);
    bool Update(UpdateOptionRequest request);
} 