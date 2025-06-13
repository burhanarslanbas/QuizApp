using QuizApp.Application.DTOs.Requests.Role;
using QuizApp.Application.DTOs.Responses.Role;

namespace QuizApp.Application.Services;

public interface IRoleService
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
    Task<RoleResponse> GetRoleByIdAsync(Guid roleId);
    Task<RoleResponse> GetRoleByNameAsync(string roleName);
    Task<List<RoleResponse>> GetAllRolesAsync();
    Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request);
    Task DeleteRoleAsync(Guid roleId);
    Task<RoleResponse> AssignRoleToUserAsync(AssignRoleRequest request);
    Task<RoleResponse> RemoveRoleFromUserAsync(RemoveRoleRequest request);
    Task<List<string>> GetUserRolesAsync(string userId);
    Task<List<string>> GetRoleClaimsAsync(string roleName);
    Task<RoleResponse> AssignClaimsToRoleAsync(string roleName, IEnumerable<string> claims);
}