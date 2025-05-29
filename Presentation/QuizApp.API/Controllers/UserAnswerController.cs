using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAnswerController : ControllerBase
{
    private readonly IUserAnswerService _userAnswerService;
    public UserAnswerController(IUserAnswerService userAnswerService)
    {
        _userAnswerService = userAnswerService;
    }

    /// <summary>
    /// Tüm kullanıcı cevaplarını getirir.
    /// </summary>
    [HttpGet("GetAll")]
    public ActionResult<List<UserAnswerDetailResponse>> GetAll([FromQuery] GetUserAnswersRequest request)
    {
        var userAnswers = _userAnswerService.GetAll(request);
        return Ok(userAnswers);
    }

    /// <summary>
    /// Belirli bir kullanıcı cevabı bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<UserAnswerDetailResponse>> GetById(Guid id)
    {
        var userAnswer = await _userAnswerService.GetByIdAsync(new GetUserAnswerByIdRequest { Id = id });
        return Ok(userAnswer);
    }

    /// <summary>
    /// Yeni bir kullanıcı cevabı oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<UserAnswerDetailResponse>> Create([FromBody] CreateUserAnswerRequest request)
    {
        var result = await _userAnswerService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Kullanıcı cevabı bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<UserAnswerDetailResponse> Update([FromBody] UpdateUserAnswerRequest request)
    {
        var result = _userAnswerService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir kullanıcı cevabını siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userAnswerService.DeleteAsync(new DeleteUserAnswerRequest { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Birden fazla kullanıcı cevabını siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] DeleteRangeUserAnswerRequest request)
    {
        var result = _userAnswerService.DeleteRange(request);
        return result ? NoContent() : BadRequest();
    }
}