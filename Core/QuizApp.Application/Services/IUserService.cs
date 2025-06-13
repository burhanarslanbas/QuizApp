using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.User;

namespace QuizApp.Application.Services;

public interface IUserService
{
    Task<UserResponse> GetCurrentUserAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
    Task<List<UserResponse>> GetAllAsync();
    Task<bool> UpdateAsync(UpdateUserRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}