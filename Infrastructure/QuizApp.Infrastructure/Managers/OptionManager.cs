using AutoMapper;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Requests.Option.Read;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class OptionManager : IOptionService
{
    private readonly IOptionWriteRepository _optionWriteRepository;
    private readonly IOptionReadRepository _optionReadRepository;
    private readonly IMapper _mapper;

    public OptionManager(IOptionWriteRepository optionWriteRepository, IOptionReadRepository optionReadRepository, IMapper mapper)
    {
        _optionWriteRepository = optionWriteRepository;
        _optionReadRepository = optionReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateOptionRequest request)
    {
        var option = _mapper.Map<Option>(request);
        return await _optionWriteRepository.AddAsync(option);
    }

    public Task<List<OptionDTO>> CreateRangeAsync(List<CreateOptionRequest> requests)
    {
        var options = _mapper.Map<List<Option>>(requests);
        return _optionWriteRepository.AddRangeAsync(options)
            .ContinueWith(task => task.Result ? _mapper.Map<List<OptionDTO>>(options) : null);
    }

    public Task<bool> Delete(Guid id)
    {
        return _optionWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var options = _optionReadRepository.GetWhere(o => ids.Contains(o.Id)).ToList();
        if (options == null || !options.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_optionWriteRepository.RemoveRange(options));
    }

    public Task<List<OptionDTO>> GetAll()
    {
        var options = _optionReadRepository.GetAll();
        if (options == null || !options.Any())
        {
            return Task.FromResult(new List<OptionDTO>());
        }
        return Task.FromResult(_mapper.Map<List<OptionDTO>>(options));
    }

    public Task<OptionDTO> GetById(Guid id)
    {
        var option = _optionReadRepository.GetByIdAsync(id).Result;
        if (option == null)
        {
            return Task.FromResult<OptionDTO>(null);
        }
        return Task.FromResult(_mapper.Map<OptionDTO>(option));
    }

    public Task<OptionDTO> Update(UpdateOptionRequest request)
    {
        var option = _mapper.Map<Option>(request);
        if (_optionWriteRepository.Update(option))
        {
            return Task.FromResult(_mapper.Map<OptionDTO>(option));
        }
        throw new Exception("Option update failed.");
    }

    public Task<List<OptionDTO>> GetByQuestionId(Guid questionId)
    {
        var options = _optionReadRepository.GetWhere(o => o.QuestionId == questionId)
            .OrderBy(o => o.OrderIndex)
            .ToList();
        return Task.FromResult(_mapper.Map<List<OptionDTO>>(options));
    }

    public async Task<bool> ReorderOptions(Guid questionId, List<Guid> optionIds)
    {
        var options = await _optionReadRepository.GetWhere(o => o.QuestionId == questionId).ToListAsync();
        if (options.Count != optionIds.Count)
            return false;

        for (int i = 0; i < optionIds.Count; i++)
        {
            var option = options.FirstOrDefault(o => o.Id == optionIds[i]);
            if (option == null)
                return false;

            option.OrderIndex = (byte)i;
        }

        return _optionWriteRepository.UpdateRange(options);
    }

    public async Task<bool> ValidateOption(Guid optionId)
    {
        var option = await _optionReadRepository.GetByIdAsync(optionId);
        return option?.IsCorrect ?? false;
    }

    public async Task<List<OptionDTO>> GetCorrectOptions(Guid questionId)
    {
        var options = await _optionReadRepository.GetWhere(o => 
            o.QuestionId == questionId && 
            o.IsCorrect)
            .OrderBy(o => o.OrderIndex)
            .ToListAsync();

        return _mapper.Map<List<OptionDTO>>(options);
    }

    public async Task<bool> HasCorrectOption(Guid questionId)
    {
        return await _optionReadRepository.GetWhere(o => 
            o.QuestionId == questionId && 
            o.IsCorrect)
            .AnyAsync();
    }
} 