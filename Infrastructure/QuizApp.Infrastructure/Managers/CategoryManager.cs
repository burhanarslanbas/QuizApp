using AutoMapper;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryWriteRepository _categoryWriteRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IMapper _mapper;

    public CategoryManager(ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository, IMapper mapper)
    {
        _categoryWriteRepository = categoryWriteRepository;
        _categoryReadRepository = categoryReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateCategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        return await _categoryWriteRepository.AddAsync(category);
    }

    public Task<List<CategoryDTO>> CreateRangeAsync(List<CreateCategoryRequest> requests)
    {
        var categories = _mapper.Map<List<Category>>(requests);
        if (categories == null || !categories.Any())
        {
            return Task.FromResult(new List<CategoryDTO>());
        }
        var result = _categoryWriteRepository.AddRangeAsync(categories);
        if (result.Result)
        {
            return Task.FromResult(_mapper.Map<List<CategoryDTO>>(categories));
        }
        throw new Exception("Category creation failed.");
    }

    public Task<bool> Delete(Guid id)
    {
        return _categoryWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var categories = _categoryReadRepository.GetWhere(c => ids.Contains(c.Id)).ToList();
        if (categories == null || !categories.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_categoryWriteRepository.RemoveRange(categories));
    }

    public Task<List<CategoryDTO>> GetAll()
    {
        var categories = _categoryReadRepository.GetAll();
        if (categories == null || !categories.Any())
        {
            return Task.FromResult(new List<CategoryDTO>());
        }
        return Task.FromResult(_mapper.Map<List<CategoryDTO>>(categories));
    }

    public Task<CategoryDTO> GetById(Guid id)
    {
        var category = _categoryReadRepository.GetByIdAsync(id).Result;
        if (category == null)
        {
            return Task.FromResult<CategoryDTO>(null);
        }
        return Task.FromResult(_mapper.Map<CategoryDTO>(category));
    }

    public Task<CategoryDTO> Update(UpdateCategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        if (_categoryWriteRepository.Update(category))
        {
            return Task.FromResult(_mapper.Map<CategoryDTO>(category));
        }
        throw new Exception("Category update failed.");
    }
}
