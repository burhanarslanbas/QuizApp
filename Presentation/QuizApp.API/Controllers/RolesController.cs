using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Role;
using QuizApp.Application.DTOs.Responses.Role;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConstants.Roles.Admin)]
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
    /// Yeni bir rol oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var result = await _roleService.CreateRoleAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(error);

        return Ok(result);
    }

    /// <summary>
    /// Rolü günceller
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        var result = await _roleService.UpdateRoleAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(error);

        return Ok(result);
    }

    /// <summary>
    /// Rolü siler
    /// </summary>
    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(Guid roleId)
    {
        await _roleService.DeleteRoleAsync(roleId);
        return Ok();
    }

    /// <summary>
    /// Id ile rolü getirir
    /// </summary>
    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetRoleById(Guid roleId)
    {
        var result = await _roleService.GetRoleByIdAsync(roleId);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(error);

        return Ok(result);
    }

    /// <summary>
    /// Rol ismine göre rolü getirir
    /// </summary>
    [HttpGet("by-name/{roleName}")]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        var result = await _roleService.GetRoleByNameAsync(roleName);
        return Ok(result);
    }

    /// <summary>
    /// Tüm rolleri getirir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Kullanıcıya rol atar
    /// </summary>
    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return NotFound("User not found");

        var result = await _roleService.AssignRoleToUserAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(error);

        return Ok(result);
    }

    /// <summary>
    /// Kullanıcıdan rolü kaldırır
    /// </summary>
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return NotFound("User not found");

        var result = await _roleService.RemoveRoleFromUserAsync(request);
        if (result is RoleErrorResponse error && error.Errors.Any())
            return BadRequest(error);

        return Ok(result);
    }

    /// <summary>
    /// Kullanıcının rollerini getirir
    /// </summary>
    [HttpGet("user-roles/{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        var roles = await _roleService.GetUserRolesAsync(userId);
        return Ok(roles);
    }

    /// <summary>
    /// Rolün claimlerini getirir
    /// </summary>
    [HttpGet("claims/{roleName}")]
    public async Task<IActionResult> GetRoleClaims(string roleName)
    {
        var result = await _roleService.GetRoleClaimsAsync(roleName);
        return Ok(result);
    }

    /// <summary>
    /// Role claim atar
    /// </summary>
    [HttpPost("assign-claims/{roleName}")]
    public async Task<IActionResult> AssignClaimsToRole(string roleName, [FromBody] IEnumerable<string> claims)
    {
        var result = await _roleService.AssignClaimsToRoleAsync(roleName, claims);
        return Ok(result);
    }
} 