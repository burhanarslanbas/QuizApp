using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/questions")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    /// <summary>
    /// Get a question by ID
    /// </summary>
    /// <param name="questionId">Question ID</param>
    /// <returns>Question details</returns>
    [HttpGet("{questionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(QuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid questionId)
    {
        var result = await _questionService.GetByIdAsync(new GetQuestionByIdRequest(questionId));
        return Ok(result);
    }

    /// <summary>
    /// Get all questions with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of questions</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllQuestions([FromQuery] GetQuestionsRequest request)
    {
        var result = await _questionService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get questions by category
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>List of questions in the category</returns>
    [HttpGet("by-category/{categoryId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoryAsync([FromRoute] Guid categoryId)
    {
        var result = await _questionService.GetByCategoryAsync(new GetQuestionsByCategoryRequest(categoryId)); 
        return Ok(result);
    }

    /// <summary>
    /// Create a new question
    /// </summary>
    /// <param name="request">Question information</param>
    /// <returns>Created question</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        var result = await _questionService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing question
    /// </summary>
    /// <param name="request">Updated question information</param>
    /// <returns>Updated question</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(QuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest request)
    {
        var result = await _questionService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a question
    /// </summary>
    /// <param name="questionId">Question ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{questionId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteQuestion([FromRoute] Guid questionId)
    {
        await _questionService.DeleteAsync(new DeleteQuestionRequest(questionId));
        return NoContent();
    }
}