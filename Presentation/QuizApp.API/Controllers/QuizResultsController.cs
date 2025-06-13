using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using QuizApp.Application.Exceptions;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/quiz-results")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class QuizResultsController : ControllerBase
{
    private readonly IQuizResultService _quizResultService;

    public QuizResultsController(IQuizResultService quizResultService)
    {
        _quizResultService = quizResultService;
    }

    /// <summary>
    /// Get a quiz result by ID
    /// </summary>
    /// <param name="quizResultId">Quiz result ID</param>
    /// <returns>Quiz result details</returns>
    [HttpGet("{quizResultId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(QuizResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuizResultById([FromRoute] Guid quizResultId)
    {
        var result = await _quizResultService.GetByIdAsync(new GetQuizResultByIdRequest(quizResultId));
        return Ok(result);
    }

    /// <summary>
    /// Get all quiz results with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of quiz results</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResultResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllQuizResults([FromQuery] GetQuizResultsRequest request)
    {
        var result = await _quizResultService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get quiz results by quiz ID
    /// </summary>
    /// <param name="quizId">Quiz ID</param>
    /// <returns>List of quiz results for the quiz</returns>
    [HttpGet("by-quiz/{quizId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResultResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResultsByQuiz([FromRoute] Guid quizId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var results = await _quizResultService.GetByQuizAsync(new GetQuizResultsByQuizRequest { QuizId = quizId });
        return Ok(results);
    }

    /// <summary>
    /// Get quiz results by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of quiz results for the user</returns>
    [HttpGet("by-user/{userId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResultResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetResultsByUser([FromRoute] Guid userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        // Students can only view their own results
        if (User.IsInRole(RoleConstants.Roles.Student) && Guid.Parse(currentUserId) != userId)
            throw new ForbiddenAccessException("You can only view your own results");

        var results = await _quizResultService.GetByUserAsync(new GetQuizResultsByUserRequest { UserId = userId });
        return Ok(results);
    }

    /// <summary>
    /// Get quiz results for the current user
    /// </summary>
    /// <returns>List of quiz results for the current user</returns>
    [HttpGet("my-results")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResultResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyResults()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var results = await _quizResultService.GetByUserAsync(new GetQuizResultsByUserRequest { UserId = Guid.Parse(userId) });
        return Ok(results);
    }

    /// <summary>
    /// Create a new quiz result
    /// </summary>
    /// <param name="request">Quiz result information</param>
    /// <returns>Created quiz result</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(QuizResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateQuizResult([FromBody] CreateQuizResultRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        request.UserId = Guid.Parse(userId);
        var result = await _quizResultService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing quiz result
    /// </summary>
    /// <param name="request">Updated quiz result information</param>
    /// <returns>Updated quiz result</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuizResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateQuizResult([FromBody] UpdateQuizResultRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var result = await _quizResultService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a quiz result
    /// </summary>
    /// <param name="quizResultId">Quiz result ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{quizResultId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteQuizResult([FromRoute] Guid quizResultId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        await _quizResultService.DeleteAsync(new DeleteQuizResultRequest { Id = quizResultId });
        return NoContent();
    }
}