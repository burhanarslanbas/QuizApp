using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.Option;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class OptionManager : IOptionService
{
    private readonly IOptionReadRepository _optionReadRepository;
    private readonly IOptionWriteRepository _optionWriteRepository;
    private readonly IMapper _mapper;

    public OptionManager(
        IOptionReadRepository optionReadRepository,
        IOptionWriteRepository optionWriteRepository,
        IMapper mapper)
    {
        _optionReadRepository = optionReadRepository;
        _optionWriteRepository = optionWriteRepository;
        _mapper = mapper;
    }

    public async Task<OptionResponse> CreateAsync(CreateOptionRequest request)
    {
        var option = _mapper.Map<Option>(request);
        await _optionWriteRepository.AddAsync(option);
        await _optionWriteRepository.SaveAsync();
        return _mapper.Map<OptionResponse>(option);
    }

    public async Task<OptionResponse> UpdateAsync(UpdateOptionRequest request)
    {
        var option = await _optionReadRepository.GetByIdAsync(request.Id);
        if (option == null)
            throw new NotFoundException($"Option with ID {request.Id} not found");

        option.QuestionId = request.QuestionId;
        option.OptionText = request.OptionText;
        option.OrderIndex = request.OrderIndex;
        option.IsCorrect = request.IsCorrect;

        _optionWriteRepository.Update(option);
        await _optionWriteRepository.SaveAsync();
        return _mapper.Map<OptionResponse>(option);
    }

    public async Task DeleteAsync(DeleteOptionRequest request)
    {
        var option = await _optionReadRepository.GetByIdAsync(request.Id);
        if (option == null)
            throw new NotFoundException($"Option with ID {request.Id} not found");

        _optionWriteRepository.Remove(option);
        await _optionWriteRepository.SaveAsync();
    }

    public async Task<OptionResponse> GetByIdAsync(GetOptionByIdRequest request)
    {
        var option = await _optionReadRepository.GetAll()
            .Include(o => o.Question)
            .FirstOrDefaultAsync(o => o.Id == request.Id);

        if (option == null)
            throw new NotFoundException($"Option with ID {request.Id} not found");

        return _mapper.Map<OptionResponse>(option);
    }

    public async Task<IEnumerable<OptionResponse>> GetAllAsync(GetOptionsRequest request)
    {
        var query = _optionReadRepository.GetAll()
            .Include(o => o.Question);

        var options = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OptionResponse>>(options);
    }

    public async Task<IEnumerable<OptionResponse>> GetByQuestionAsync(GetOptionsByQuestionRequest request)
    {
        var options = await _optionReadRepository.GetAll()
            .Include(o => o.Question)
            .Where(o => o.QuestionId == request.QuestionId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OptionResponse>>(options);
    }
} 