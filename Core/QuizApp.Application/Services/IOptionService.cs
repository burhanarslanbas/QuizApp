using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.Services;

public interface IOptionService
{
    Task<OptionResponse> CreateAsync(CreateOptionRequest request);
    Task<OptionResponse> UpdateAsync(UpdateOptionRequest request);
    Task DeleteAsync(DeleteOptionRequest request);
    Task<OptionResponse> GetByIdAsync(GetOptionByIdRequest request);
    Task<IEnumerable<OptionResponse>> GetAllAsync(GetOptionsRequest request);
    Task<IEnumerable<OptionResponse>> GetByQuestionAsync(GetOptionsByQuestionRequest request);
}