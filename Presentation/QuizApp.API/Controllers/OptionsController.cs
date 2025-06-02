using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OptionsController : ControllerBase
{
    private readonly IOptionService _optionService;

    public OptionsController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    [HttpGet("{optionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public async Task<IActionResult> GetOptionById([FromRoute] Guid optionId)
    {
        var result = await _optionService.GetByIdAsync(optionId);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public IActionResult GetAllOptions()
    {
        var result = _optionService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> CreateOption([FromBody] CreateOptionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.CreateAsync(request, Guid.Parse(userId));
        return Ok(result);
    }

    [HttpPost("range")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateOptionRequest> requests)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.CreateRangeAsync(requests, Guid.Parse(userId));
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> UpdateOption([FromBody] UpdateOptionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.Update(request, Guid.Parse(userId));
        return Ok(result);
    }

    [HttpDelete("{optionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> DeleteOption([FromRoute] Guid optionId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.DeleteAsync(optionId, Guid.Parse(userId));
        return Ok(result);
    }

    [HttpDelete("range")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteRange([FromBody] List<Guid> ids)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.DeleteRange(ids, Guid.Parse(userId));
        return Ok(result);
    }
} 