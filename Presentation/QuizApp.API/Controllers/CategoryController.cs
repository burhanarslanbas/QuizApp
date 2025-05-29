using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Tüm kategorileri getirir.
    /// </summary>
    [HttpGet("GetAll")]
    [Authorize(AuthenticationSchemes = "Admin")] // Örnek olarak sadece Admin ve User rolleri erişebilir
    public ActionResult<List<CategoryDetailResponse>> GetAll()
    {
        var categories = _categoryService.GetAll(new GetCategoriesRequest());
        return Ok(categories);
    }

    /// <summary>
    /// Belirli bir kategori bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<CategoryDetailResponse>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(new GetCategoryByIdRequest { Id = id });
        return Ok(category);
    }

    /// <summary>
    /// Yeni bir kategori oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<CategoryDetailResponse>> Create([FromBody] CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Birden fazla kategori oluşturur.
    /// </summary>
    [HttpPost("CreateRange")]
    public async Task<ActionResult<List<CategoryDetailResponse>>> CreateRange([FromBody] CreateRangeCategoryRequest request)
    {
        var result = await _categoryService.CreateRange(request);
        return Ok(result);
    }

    /// <summary>
    /// Kategori bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<CategoryDetailResponse> Update([FromBody] UpdateCategoryRequest request)
    {
        var result = _categoryService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Birden fazla kategoriyi günceller.
    /// </summary>
    [HttpPut("UpdateRange")]
    public ActionResult<List<CategoryDetailResponse>> UpdateRange([FromBody] UpdateRangeCategoryRequest request)
    {
        var result = _categoryService.UpdateRange(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir kategoriyi siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(new DeleteCategoryRequest { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Birden fazla kategoriyi siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] DeleteRangeCategoryRequest request)
    {
        var result = _categoryService.DeleteRange(request);
        return result ? NoContent() : BadRequest();
    }
}