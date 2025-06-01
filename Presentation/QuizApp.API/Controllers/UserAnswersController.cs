using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.Services;

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

    /// <summary>
    /// Id ile kullanıcı cevabı getirir
    /// </summary>
    [HttpGet("{userAnswerId}")]
    public async Task<IActionResult> GetUserAnswerById([FromRoute] GetUserAnswerByIdRequest userAnswerId)
    {
        var result = await _userAnswerService.GetByIdAsync(userAnswerId);
        return Ok(result);
    }

    /// <summary>
    /// Tüm kullanıcı cevaplarını getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllUserAnswers()
    {
        var result = _userAnswerService.GetAll(new GetUserAnswersRequest());
        return Ok(result);
    }

    /// <summary>
    /// Quiz sonucuna göre kullanıcı cevaplarını getirir
    /// </summary>
    [HttpGet("by-quiz-result")]
    public IActionResult GetUserAnswersByQuizResult([FromQuery] GetUserAnswersByQuizResultRequest request)
    {
        var result = _userAnswerService.GetByQuizResult(request);
        return Ok(result);
    }

    /// <summary>
    /// Yeni kullanıcı cevabı oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUserAnswer([FromBody] CreateUserAnswerRequest request)
    {
        var result = await _userAnswerService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Kullanıcı cevabı günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateUserAnswer([FromBody] UpdateUserAnswerRequest request)
    {
        var result = _userAnswerService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Kullanıcı cevabı siler
    /// </summary>
    [HttpDelete("{userAnswerId}")]
    public async Task<IActionResult> DeleteUserAnswer([FromRoute] DeleteUserAnswerRequest userAnswerId)
    {
        await _userAnswerService.DeleteAsync(userAnswerId);
        return Ok();
    }

    /// <summary>
    /// Çoklu kullanıcı cevabı siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeUserAnswerRequest ids)
    {
        var result = _userAnswerService.DeleteRange(ids);
        return Ok(result);
    }
}