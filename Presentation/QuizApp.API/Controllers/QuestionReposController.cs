using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/question-repos")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class QuestionReposController : ControllerBase
{
    private readonly IQuestionRepoService _questionRepoService;

    public QuestionReposController(IQuestionRepoService questionRepoService)
    {
        _questionRepoService = questionRepoService;
    }

    /// <summary>
    /// Get a question repository by ID
    /// </summary>
    /// <param name="questionRepoId">Question repository ID</param>
    /// <returns>Question repository details</returns>
    [HttpGet("{questionRepoId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuestionRepoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetQuestionRepoById([FromRoute] Guid questionRepoId)
    {
        var result = await _questionRepoService.GetByIdAsync(new GetQuestionRepoByIdRequest(questionRepoId));
        return Ok(result);
    }

    /// <summary>
    /// Get all question repositories with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of question repositories</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(IEnumerable<QuestionRepoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllQuestionRepos([FromQuery] GetQuestionReposRequest request)
    {
        var result = await _questionRepoService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Create a new question repository
    /// </summary>
    /// <param name="request">Question repository information</param>
    /// <returns>Created question repository</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuestionRepoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateQuestionRepo([FromBody] CreateQuestionRepoRequest request)
    {
        var result = await _questionRepoService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing question repository
    /// </summary>
    /// <param name="request">Updated question repository information</param>
    /// <returns>Updated question repository</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuestionRepoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateQuestionRepo([FromBody] UpdateQuestionRepoRequest request)
    {
        var result = await _questionRepoService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a question repository
    /// </summary>
    /// <param name="questionRepoId">Question repository ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{questionRepoId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteQuestionRepo([FromRoute] Guid questionRepoId)
    {
        await _questionRepoService.DeleteAsync(new DeleteQuestionRepoRequest(questionRepoId));
        return NoContent();
    }

    /// <summary>
    /// Delete multiple question repositories
    /// </summary>
    /// <param name="request">List of question repository IDs to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("range")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeQuestionRepoRequest request)
    {
        await _questionRepoService.DeleteRangeAsync(request);
        return NoContent();
    }
}