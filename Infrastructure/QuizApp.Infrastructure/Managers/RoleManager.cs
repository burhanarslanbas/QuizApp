using Microsoft.AspNetCore.Identity;
using QuizApp.Application.DTOs.Requests.Role;
using QuizApp.Application.DTOs.Responses.Role;
using QuizApp.Application.Services;
using QuizApp.Application.Exceptions;
using QuizApp.Domain.Entities.Identity;
using System.Security.Claims;

namespace QuizApp.Infrastructure.Managers;

public class RoleManager : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public RoleManager(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<RoleResponse> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            LastModifiedDate = role.LastModifiedDate,
            IsActive = role.IsActive,
            Claims = claims.Select(c => c.Value).ToList()
        };
    }

    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
    {
        var role = new AppRole
        {
            Name = request.Name,
            Description = request.Description
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            throw new BusinessException($"Role creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (request.Claims.Any())
        {
            await AssignClaimsToRoleAsync(role.Name!, request.Claims);
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            IsActive = role.IsActive,
            Claims = request.Claims
        };
    }

    public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        role.Name = request.Name;
        role.Description = request.Description;
        role.IsActive = request.IsActive;
        role.LastModifiedDate = DateTime.UtcNow;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            throw new BusinessException($"Role update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (request.Claims.Any())
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in currentClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            await AssignClaimsToRoleAsync(role.Name!, request.Claims);
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            LastModifiedDate = role.LastModifiedDate,
            IsActive = role.IsActive,
            Claims = request.Claims
        };
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role != null)
        {
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new BusinessException($"Role deletion failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }

    public async Task<RoleResponse> GetRoleByIdAsync(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            LastModifiedDate = role.LastModifiedDate,
            IsActive = role.IsActive,
            Claims = claims.Select(c => c.Value).ToList()
        };
    }

    public async Task<List<RoleResponse>> GetAllRolesAsync()
    {
        var roles = _roleManager.Roles.ToList();
        var roleResponses = new List<RoleResponse>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            roleResponses.Add(new RoleResponse
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Description = role.Description,
                CreatedDate = role.CreatedDate,
                LastModifiedDate = role.LastModifiedDate,
                IsActive = role.IsActive,
                Claims = claims.Select(c => c.Value).ToList()
            });
        }

        return roleResponses;
    }

    public async Task<List<string>> GetRoleClaimsAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return new List<string>();
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        return claims.Select(c => c.Value).ToList();
    }

    public async Task<RoleResponse> AssignRoleToUserAsync(AssignRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        var result = await _userManager.AddToRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            throw new BusinessException($"Role assignment failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            LastModifiedDate = role.LastModifiedDate,
            IsActive = role.IsActive
        };
    }

    public async Task<RoleResponse> RemoveRoleFromUserAsync(RemoveRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            throw new BusinessException($"Role removal failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            LastModifiedDate = role.LastModifiedDate,
            IsActive = role.IsActive
        };
    }

    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new List<string>();
        }

        return (await _userManager.GetRolesAsync(user)).ToList();
    }

    public async Task<RoleResponse> AssignClaimsToRoleAsync(string roleName, IEnumerable<string> claims)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        foreach (var claim in claims)
        {
            await _roleManager.AddClaimAsync(role, new Claim("Permission", claim));
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            LastModifiedDate = role.LastModifiedDate,
            IsActive = role.IsActive,
            Claims = claims.ToList()
        };
    }
}