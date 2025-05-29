using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptionController : ControllerBase
{
    private readonly IOptionService _optionService;
    public OptionController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    /// <summary>
    /// Tüm seçenekleri getirir.
    /// </summary>
    [HttpGet("GetAll")]
    public ActionResult<List<OptionDTO>> GetAll()
    {
        var options = _optionService.GetAll();
        return Ok(options);
    }

    /// <summary>
    /// Belirli bir seçenek bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<OptionDTO>> GetById(Guid id)
    {
        var option = await _optionService.GetByIdAsync(id);
        return Ok(option);
    }

    /// <summary>
    /// Yeni bir seçenek oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<bool>> Create([FromBody] CreateOptionRequest request)
    {
        var result = await _optionService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Birden fazla seçenek oluşturur.
    /// </summary>
    [HttpPost("CreateRange")]
    public async Task<ActionResult<List<OptionDTO>>> CreateRange([FromBody] List<CreateOptionRequest> request)
    {
        var result = await _optionService.CreateRangeAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Seçenek bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<bool> Update([FromBody] UpdateOptionRequest request)
    {
        var result = _optionService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir seçeneği siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _optionService.DeleteAsync(id);
        return result ? NoContent() : BadRequest();
    }

    /// <summary>
    /// Birden fazla seçeneği siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] List<Guid> guids)
    {
        var result = _optionService.DeleteRange(guids);
        return result ? NoContent() : BadRequest();
    }
}

internal record CreatedOptionResponse(object Id);
