using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateAsync(request);
            if (result)
            {
                return Ok("Category created successfully.");
            }
            return StatusCode(500, "An error occurred while creating the category.");
        }

        // Delete a category by ID
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _categoryService.Delete(id);
            if (result)
            {
                return Ok("Category deleted successfully.");
            }
            return NotFound("Category not found.");
        }

        // Update a category
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCategoryRequest request)
        {
            var result = await _categoryService.Update(request);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Category not found.");
        }

        // Get a category by ID
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _categoryService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Category not found.");
        }
        // Get all categories
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _categoryService.GetAll();
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            return NotFound("No categories found.");
        }
        // Create multiple categories
        [HttpPost("CreateRange")]
        public async Task<IActionResult> CreateRangeAsync([FromBody] List<CreateCategoryRequest> requests)
        {
            var result = await _categoryService.CreateRangeAsync(requests);
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            return StatusCode(500, "An error occurred while creating categories.");
        }
        // Delete multiple categories
        [HttpDelete("DeleteRange")]
        public async Task<IActionResult> DeleteRangeAsync([FromBody] List<Guid> ids)
        {
            var result = await _categoryService.DeleteRange(ids);
            if (result)
            {
                return Ok("Categories deleted successfully.");
            }
            return NotFound("No categories found for the provided IDs.");
        }
    }
}
