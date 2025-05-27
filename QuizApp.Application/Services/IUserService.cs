using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.User;

namespace QuizApp.Application.Services;

public interface IUserService
{
    Task<bool> CreateAsync(CreateUserRequest request);
    Task<List<UserDTO>> CreateRangeAsync(List<CreateUserRequest> requests);
    Task<bool> DeleteAsync(Guid id);
    bool DeleteRange(List<Guid> ids);
    List<UserDTO> GetAll();
    Task<UserDTO> GetByIdAsync(Guid id);
    bool Update(UpdateUserRequest request);
} 