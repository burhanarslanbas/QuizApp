using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.Services;

public interface IOptionService
{
    Task<bool> CreateAsync(CreateOptionRequest request, Guid userId);
    Task<List<OptionDTO>> CreateRangeAsync(List<CreateOptionRequest> requests, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<bool> DeleteRange(List<Guid> ids, Guid userId);
    List<OptionDTO> GetAll();
    Task<OptionDTO> GetByIdAsync(Guid id);
    Task<bool> Update(UpdateOptionRequest request, Guid userId);
} 