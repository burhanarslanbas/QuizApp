using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuizResultsController : ControllerBase
{
    private readonly IQuizResultService _quizResultService;

    public QuizResultsController(IQuizResultService quizResultService)
    {
        _quizResultService = quizResultService;
    }

    [HttpGet("{quizResultId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public async Task<IActionResult> GetQuizResultById([FromRoute] Guid quizResultId)
    {
        var result = await _quizResultService.GetByIdAsync(quizResultId);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult GetAllQuizResults()
    {
        var result = _quizResultService.GetAll();
        return Ok(result);
    }

    [HttpGet("by-quiz/{quizId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult GetResultsByQuiz([FromRoute] Guid quizId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        var results = _quizResultService.GetByQuizIdAndOwner(quizId, Guid.Parse(userId));
        return Ok(results);
    }

    [HttpGet("by-user/{userId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public IActionResult GetResultsByUser([FromRoute] Guid userId)
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId == null)
            return Unauthorized();

        // Öğrenci sadece kendi sonuçlarını görebilir
        if (User.IsInRole(RoleConstants.Roles.Student) && Guid.Parse(currentUserId) != userId)
            return Forbid();

        var results = _quizResultService.GetByQuizIdAndOwner(userId, Guid.Parse(currentUserId));
        return Ok(results);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public async Task<IActionResult> CreateQuizResult([FromBody] CreateQuizResultRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        request.UserId = Guid.Parse(userId);
        var result = await _quizResultService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult UpdateQuizResult([FromBody] UpdateQuizResultRequest request)
    {
        var result = _quizResultService.Update(request);
        return Ok(result);
    }

    [HttpDelete("{quizResultId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> DeleteQuizResult([FromRoute] Guid quizResultId)
    {
        await _quizResultService.DeleteAsync(quizResultId);
        return Ok();
    }

    [HttpDelete("range")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult DeleteRange([FromBody] List<Guid> ids)
    {
        var result = _quizResultService.DeleteRange(ids);
        return Ok(result);
    }
}