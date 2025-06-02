using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserAnswersController : ControllerBase
{
    private readonly IUserAnswerService _userAnswerService;

    public UserAnswersController(IUserAnswerService userAnswerService)
    {
        _userAnswerService = userAnswerService;
    }

    [HttpGet("{userAnswerId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public async Task<IActionResult> GetUserAnswerById([FromRoute] GetUserAnswerByIdRequest userAnswerId)
    {
        var result = await _userAnswerService.GetByIdAsync(userAnswerId);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult GetAllUserAnswers()
    {
        var result = _userAnswerService.GetAll(new GetUserAnswersRequest());
        return Ok(result);
    }

    [HttpGet("by-quiz-result")]
    public IActionResult GetUserAnswersByQuizResult([FromQuery] GetUserAnswersByQuizResultRequest request)
    {
        var result = _userAnswerService.GetByQuizResult(request);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public async Task<IActionResult> CreateUserAnswer([FromBody] CreateUserAnswerRequest request)
    {
        var result = await _userAnswerService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult UpdateUserAnswer([FromBody] UpdateUserAnswerRequest request)
    {
        var result = _userAnswerService.Update(request);
        return Ok(result);
    }

    [HttpDelete("{userAnswerId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> DeleteUserAnswer([FromRoute] DeleteUserAnswerRequest userAnswerId)
    {
        await _userAnswerService.DeleteAsync(userAnswerId);
        return Ok();
    }

    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeUserAnswerRequest ids)
    {
        var result = _userAnswerService.DeleteRange(ids);
        return Ok(result);
    }
}