using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuizQuestion;
using QuizApp.Application.DTOs.Responses.QuizQuestion;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/quiz-questions")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class QuizQuestionsController : ControllerBase
{
    private readonly IQuizQuestionService _quizQuestionService;

    public QuizQuestionsController(IQuizQuestionService quizQuestionService)
    {
        _quizQuestionService = quizQuestionService;
    }

    /// <summary>
    /// Get a quiz question by ID
    /// </summary>
    /// <param name="quizQuestionId">Quiz question ID</param>
    /// <returns>Quiz question details</returns>
    [HttpGet("{quizQuestionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(QuizQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid quizQuestionId)
    {
        var result = await _quizQuestionService.GetByIdAsync(new GetQuizQuestionByIdRequest { Id = quizQuestionId });
        return Ok(result);
    }

    /// <summary>
    /// Get all quiz questions with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of quiz questions</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuizQuestionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetQuizQuestionsRequest request)
    {
        var result = await _quizQuestionService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get quiz questions by quiz ID
    /// </summary>
    /// <param name="quizId">Quiz ID</param>
    /// <returns>List of quiz questions for the quiz</returns>
    [HttpGet("by-quiz/{quizId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuizQuestionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByQuizAsync([FromRoute] Guid quizId)
    {
        var result = await _quizQuestionService.GetAllAsync(new GetQuizQuestionsRequest { QuizId = quizId });
        return Ok(result);
    }

    /// <summary>
    /// Create a new quiz question
    /// </summary>
    /// <param name="request">Quiz question information</param>
    /// <returns>Created quiz question</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuizQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateQuizQuestionRequest request)
    {
        var result = await _quizQuestionService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing quiz question
    /// </summary>
    /// <param name="request">Updated quiz question information</param>
    /// <returns>Updated quiz question</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuizQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdateQuizQuestionRequest request)
    {
        var result = await _quizQuestionService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a quiz question
    /// </summary>
    /// <param name="quizQuestionId">Quiz question ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{quizQuestionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] Guid quizQuestionId)
    {
        await _quizQuestionService.DeleteAsync(new DeleteQuizQuestionRequest { Id = quizQuestionId });
        return NoContent();
    }
}