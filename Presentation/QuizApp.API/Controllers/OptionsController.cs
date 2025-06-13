using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/options")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class OptionsController : ControllerBase
{
    private readonly IOptionService _optionService;

    public OptionsController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    /// <summary>
    /// Get an option by ID
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option details</returns>
    [HttpGet("{optionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(OptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOptionById([FromRoute] Guid optionId)
    {
        var result = await _optionService.GetByIdAsync(new GetOptionByIdRequest { Id = optionId });
        return Ok(result);
    }

    /// <summary>
    /// Get all options with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of options</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<OptionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOptions([FromQuery] GetOptionsRequest request)
    {
        var result = await _optionService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get options by question ID
    /// </summary>
    /// <param name="request">Question ID and pagination parameters</param>
    /// <returns>List of options for the question</returns>
    [HttpGet("by-question")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<OptionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOptionsByQuestion([FromQuery] GetOptionsByQuestionRequest request)
    {
        var result = await _optionService.GetByQuestionAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Create a new option
    /// </summary>
    /// <param name="request">Option information</param>
    /// <returns>Created option</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(OptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateOption([FromBody] CreateOptionRequest request)
    {
        var result = await _optionService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing option
    /// </summary>
    /// <param name="request">Updated option information</param>
    /// <returns>Updated option</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(OptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateOption([FromBody] UpdateOptionRequest request)
    {
        var result = await _optionService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete an option
    /// </summary>
    /// <param name="optionId">Option ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{optionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteOption([FromRoute] Guid optionId)
    {
        await _optionService.DeleteAsync(new DeleteOptionRequest { Id = optionId });
        return NoContent();
    }
}