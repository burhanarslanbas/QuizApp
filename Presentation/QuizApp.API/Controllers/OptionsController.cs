using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OptionsController : ControllerBase
{
    private readonly IOptionService _optionService;

    public OptionsController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    /// <summary>
    /// Id ile seçenek getirir
    /// </summary>
    [HttpGet("{optionId}")]
    public async Task<IActionResult> GetOptionById([FromRoute] Guid optionId)
    {
        var result = await _optionService.GetByIdAsync(optionId);
        return Ok(result);
    }

    /// <summary>
    /// Tüm seçenekleri getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllOptions()
    {
        var result = _optionService.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Yeni seçenek oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOption([FromBody] CreateOptionRequest request)
    {
        var result = await _optionService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Çoklu seçenek oluşturur
    /// </summary>
    [HttpPost("range")]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateOptionRequest> requests)
    {
        var result = await _optionService.CreateRangeAsync(requests);
        return Ok(result);
    }

    /// <summary>
    /// Seçenek günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateOption([FromBody] UpdateOptionRequest request)
    {
        var result = _optionService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Seçenek siler
    /// </summary>
    [HttpDelete("{optionId}")]
    public async Task<IActionResult> DeleteOption([FromRoute] Guid optionId)
    {
        var result = await _optionService.DeleteAsync(optionId);
        return Ok(result);
    }

    /// <summary>
    /// Çoklu seçenek siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] List<Guid> ids)
    {
        var result = _optionService.DeleteRange(ids);
        return Ok(result);
    }
} 