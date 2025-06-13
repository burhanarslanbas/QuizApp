using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Role;
using QuizApp.Application.DTOs.Responses;
using QuizApp.Application.DTOs.Responses.Role;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.API.Controllers;

[Route("api/roles")]
[ApiController]
[Authorize(Roles = RoleConstants.Roles.Admin)]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly UserManager<AppUser> _userManager;

    public RolesController(IRoleService roleService, UserManager<AppUser> userManager)
    {
        _roleService = roleService;
        _userManager = userManager;
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var result = await _roleService.CreateRoleAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(new ErrorResponse { Success = false, Message = error.Message, Errors = error.Errors });

        return Ok(result);
    }

    /// <summary>
    /// Update an existing role
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        var result = await _roleService.UpdateRoleAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(new ErrorResponse { Success = false, Message = error.Message, Errors = error.Errors });

        return Ok(result);
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    [HttpDelete("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRole(Guid roleId)
    {
        await _roleService.DeleteRoleAsync(roleId);
        return NoContent();
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    [HttpGet("{roleId}")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleById(Guid roleId)
    {
        var result = await _roleService.GetRoleByIdAsync(roleId);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(new ErrorResponse { Success = false, Message = error.Message, Errors = error.Errors });

        return Ok(result);
    }

    /// <summary>
    /// Get role by name
    /// </summary>
    [HttpGet("by-name/{roleName}")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        var result = await _roleService.GetRoleByNameAsync(roleName);
        return Ok(result);
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RoleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Assign role to user
    /// </summary>
    [HttpPost("assign")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return NotFound(new ErrorResponse { Success = false, Message = "User not found" });

        var result = await _roleService.AssignRoleToUserAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(new ErrorResponse { Success = false, Message = error.Message, Errors = error.Errors });

        return Ok(result);
    }

    /// <summary>
    /// Remove role from user
    /// </summary>
    [HttpPost("remove")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return NotFound(new ErrorResponse { Success = false, Message = "User not found" });

        var result = await _roleService.RemoveRoleFromUserAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(new ErrorResponse { Success = false, Message = error.Message, Errors = error.Errors });

        return Ok(result);
    }

    /// <summary>
    /// Get user roles
    /// </summary>
    [HttpGet("user-roles/{userId}")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound(new ErrorResponse { Success = false, Message = "User not found" });

        var roles = await _roleService.GetUserRolesAsync(userId);
        return Ok(roles);
    }

    /// <summary>
    /// Get role claims
    /// </summary>
    [HttpGet("claims/{roleName}")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleClaims(string roleName)
    {
        var result = await _roleService.GetRoleClaimsAsync(roleName);
        return Ok(result);
    }

    /// <summary>
    /// Assign claims to role
    /// </summary>
    [HttpPost("assign-claims/{roleName}")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignClaimsToRole(string roleName, [FromBody] IEnumerable<string> claims)
    {
        var result = await _roleService.AssignClaimsToRoleAsync(roleName, claims);
        return Ok(result);
    }
}