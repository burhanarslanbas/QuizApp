using AutoMapper;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Infrastructure.Managers;

public class OptionManager : IOptionService
{
    private readonly IOptionReadRepository _optionReadRepository;
    private readonly IOptionWriteRepository _optionWriteRepository;
    private readonly IMapper _mapper;

    public OptionManager(IOptionReadRepository optionReadRepository, IOptionWriteRepository optionWriteRepository, IMapper mapper)
    {
        _optionReadRepository = optionReadRepository;
        _optionWriteRepository = optionWriteRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateOptionRequest request)
    {
        var option = _mapper.Map<Option>(request);
        var result = await _optionWriteRepository.AddAsync(option);
        if (!result)
            throw new BusinessException("Failed to create option");

        return result;
    }

    public async Task<List<OptionDTO>> CreateRangeAsync(List<CreateOptionRequest> requests)
    {
        var options = _mapper.Map<List<Option>>(requests);
        var result = await _optionWriteRepository.AddRangeAsync(options);
        if (!result)
            throw new BusinessException("Failed to create options");

        return _mapper.Map<List<OptionDTO>>(options);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _optionWriteRepository.RemoveById(id);
        if (!result)
            throw new NotFoundException($"Option with ID {id} not found.");

        return result;
    }

    public bool DeleteRange(List<Guid> ids)
    {
        var options = _optionReadRepository.GetWhere(x => ids.Contains(x.Id)).ToList();
        if (!options.Any())
            throw new NotFoundException("No options found with the provided IDs.");

        var result = _optionWriteRepository.RemoveRange(options);
        if (!result)
            throw new BusinessException("Failed to delete options.");

        return result;
    }

    public List<OptionDTO> GetAll()
    {
        var options = _optionReadRepository.GetAll().ToList();
        return _mapper.Map<List<OptionDTO>>(options);
    }

    public async Task<OptionDTO> GetByIdAsync(Guid id)
    {
        try
        {
            var option = await _optionReadRepository.GetByIdAsync(id);
            return _mapper.Map<OptionDTO>(option);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Option with ID {id} not found.");
        }
    }

    public bool Update(UpdateOptionRequest request)
    {
        try
        {
            var option = _optionReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, option);
            var result = _optionWriteRepository.Update(option);
            if (!result)
                throw new BusinessException($"Failed to update option with ID {request.Id}");

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Option with ID {request.Id} not found.");
        }
    }
}