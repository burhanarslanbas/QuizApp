using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using QuizApp.Application.Exceptions;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/quizzes")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    /// <summary>
    /// Get a quiz by ID
    /// </summary>
    /// <param name="quizId">Quiz ID</param>
    /// <returns>Quiz details</returns>
    [HttpGet("{quizId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(QuizResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuizById([FromRoute] Guid quizId)
    {
        var result = await _quizService.GetByIdAsync(new GetQuizByIdRequest { Id = quizId });
        return Ok(result);
    }

    /// <summary>
    /// Get all quizzes with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of quizzes</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllQuizzes([FromQuery] GetQuizzesRequest request)
    {
        var result = await _quizService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get quizzes by category
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>List of quizzes in the category</returns>
    [HttpGet("by-category/{categoryId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoryAsync([FromRoute] Guid categoryId)
    {
        var result = await _quizService.GetByCategoryAsync(new GetQuizzesByCategoryRequest { CategoryId = categoryId });
        return Ok(result);
    }

    /// <summary>
    /// Get quizzes by user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of quizzes created by the user</returns>
    [HttpGet("by-user/{userId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserAsync([FromRoute] Guid userId)
    {
        var result = await _quizService.GetByUserAsync(new GetQuizzesByUserRequest(userId));
        return Ok(result);
    }

    /// <summary>
    /// Get quizzes created by the current user
    /// </summary>
    /// <returns>List of quizzes created by the current user</returns>
    [HttpGet("my-quizzes")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(IEnumerable<QuizResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyQuizzes()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var result = await _quizService.GetByUserAsync(new GetQuizzesByUserRequest(Guid.Parse(userId)));
        return Ok(result);
    }

    /// <summary>
    /// Create a new quiz
    /// </summary>
    /// <param name="request">Quiz information</param>
    /// <returns>Created quiz</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuizResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        request.CreatorId = Guid.Parse(userId);
        var result = await _quizService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing quiz
    /// </summary>
    /// <param name="request">Updated quiz information</param>
    /// <returns>Updated quiz</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuizResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var result = await _quizService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a quiz
    /// </summary>
    /// <param name="quizId">Quiz ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{quizId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteQuiz([FromRoute] Guid quizId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        await _quizService.DeleteAsync(new DeleteQuizRequest(quizId));
        return NoContent();
    }
}