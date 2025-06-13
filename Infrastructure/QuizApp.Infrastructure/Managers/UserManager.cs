using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.User;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities.Identity;
using System.Security.Claims;

namespace QuizApp.Infrastructure.Managers;

public class UserManager : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManager(
        UserManager<AppUser> userManager,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserResponse> GetCurrentUserAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
        if (user == null)
            throw new NotFoundException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList();

        var userDto = _mapper.Map<UserResponse>(user);
        userDto.Roles = roles.ToList();
        userDto.Claims = claims;

        return userDto;
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {id} not found");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList();

        var userDto = _mapper.Map<UserResponse>(user);
        userDto.Roles = roles.ToList();
        userDto.Claims = claims;

        return userDto;
    }

    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList();

            var userDto = _mapper.Map<UserResponse>(user);
            userDto.Roles = roles.ToList();
            userDto.Claims = claims;

            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<bool> UpdateAsync(UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {request.Id} not found");

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.UserName = request.UserName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new BusinessException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            var changePasswordResult = await ChangePasswordAsync(request.Id, request.CurrentPassword!, request.NewPassword);
            if (!changePasswordResult)
                throw new BusinessException("Failed to change password");
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {id} not found");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new BusinessException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        return true;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
            throw new BusinessException($"Failed to change password: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        return true;
    }

    public async Task<AppUser?> GetUserAsync(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            return null;
        }
        return await _userManager.GetUserAsync(principal);
    }
}