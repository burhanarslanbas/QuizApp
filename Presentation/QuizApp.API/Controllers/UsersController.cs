using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.User;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using QuizApp.Application.DTOs.Responses;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    /// <returns>Current user details</returns>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            return Ok(user);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Success = false, Message = "User is not authenticated" });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = RoleConstants.Roles.Admin)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Success = false, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet]
    [Authorize(Roles = RoleConstants.Roles.Admin)]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Update user information
    /// </summary>
    /// <param name="request">Updated user information</param>
    /// <returns>Updated user details</returns>
    [HttpPut]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
    {
        try
        {
            var result = await _userService.UpdateAsync(request);
            var updatedUser = await _userService.GetByIdAsync(request.Id);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Success = false, Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Success = false, Message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = RoleConstants.Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse { Success = false, Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Success = false, Message = ex.Message });
        }
    }
}