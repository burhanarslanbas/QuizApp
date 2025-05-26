using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories;

namespace QuizApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        public CategoryController(ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
        }

        [HttpGet]
        public async Task Get()
        {

        }
        // Oluşturduğumuz verileri json formatında görebilmek için bu fonksiyonu oluşturduk.
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryReadRepository.Table.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryReadRepository.GetByIdAsync(id);
            return Ok(category);
        }
    }
}
