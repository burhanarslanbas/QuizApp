using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/user-answers")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class UserAnswersController : ControllerBase
{
    private readonly IUserAnswerService _userAnswerService;

    public UserAnswersController(IUserAnswerService userAnswerService)
    {
        _userAnswerService = userAnswerService;
    }

    /// <summary>
    /// Get a user answer by ID
    /// </summary>
    /// <param name="userAnswerId">User answer ID</param>
    /// <returns>User answer details</returns>
    [HttpGet("{userAnswerId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(UserAnswerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserAnswerById([FromRoute] Guid userAnswerId)
    {
        var result = await _userAnswerService.GetByIdAsync(new GetUserAnswerByIdRequest { Id = userAnswerId });
        return Ok(result);
    }

    /// <summary>
    /// Get all user answers with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of user answers</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(IEnumerable<UserAnswerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUserAnswers([FromQuery] GetUserAnswersRequest request)
    {
        var result = await _userAnswerService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get user answers by quiz result
    /// </summary>
    /// <param name="request">Quiz result information</param>
    /// <returns>List of user answers for the quiz result</returns>
    [HttpGet("by-quiz-result")]
    [ProducesResponseType(typeof(IEnumerable<UserAnswerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserAnswersByQuizResult([FromQuery] GetUserAnswersByQuizResultRequest request)
    {
        var result = await _userAnswerService.GetByQuizResultAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Create a new user answer
    /// </summary>
    /// <param name="request">User answer information</param>
    /// <returns>Created user answer</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(UserAnswerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateUserAnswer([FromBody] CreateUserAnswerRequest request)
    {
        var result = await _userAnswerService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing user answer
    /// </summary>
    /// <param name="request">Updated user answer information</param>
    /// <returns>Updated user answer</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(UserAnswerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserAnswer([FromBody] UpdateUserAnswerRequest request)
    {
        var result = await _userAnswerService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a user answer
    /// </summary>
    /// <param name="userAnswerId">User answer ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{userAnswerId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteUserAnswer([FromRoute] Guid userAnswerId)
    {
        await _userAnswerService.DeleteAsync(new DeleteUserAnswerRequest { Id = userAnswerId });
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeUserAnswerRequest ids)
    {
        await _userAnswerService.DeleteRangeAsync(ids);
        return Ok();
    }
}