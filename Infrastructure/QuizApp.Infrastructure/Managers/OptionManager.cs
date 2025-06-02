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
    private readonly IQuestionReadRepository _questionReadRepository;
    private readonly IQuizReadRepository _quizReadRepository;
    private readonly IMapper _mapper;

    public OptionManager(
        IOptionReadRepository optionReadRepository, 
        IOptionWriteRepository optionWriteRepository,
        IQuestionReadRepository questionReadRepository,
        IQuizReadRepository quizReadRepository,
        IMapper mapper)
    {
        _optionReadRepository = optionReadRepository;
        _optionWriteRepository = optionWriteRepository;
        _questionReadRepository = questionReadRepository;
        _quizReadRepository = quizReadRepository;
        _mapper = mapper;
    }

    private async Task CheckQuizOwnership(Guid questionId, Guid userId)
    {
        var question = await _questionReadRepository.GetByIdAsync(questionId);
        if (question == null)
            throw new NotFoundException($"Question with ID {questionId} not found.");

        var quiz = await _quizReadRepository.GetByIdAsync(question.QuizId);
        if (quiz == null)
            throw new NotFoundException($"Quiz with ID {question.QuizId} not found.");
        
        if (quiz.CreatorId != userId)
            throw new UnauthorizedException("You are not authorized to modify this quiz's options.");
    }

    public async Task<bool> CreateAsync(CreateOptionRequest request, Guid userId)
    {
        await CheckQuizOwnership(request.QuestionId, userId);

        var option = _mapper.Map<Option>(request);
        var result = await _optionWriteRepository.AddAsync(option);
        if (!result)
            throw new BusinessException("Failed to create option");

        return result;
    }

    public async Task<List<OptionDTO>> CreateRangeAsync(List<CreateOptionRequest> requests, Guid userId)
    {
        if (!requests.Any())
            throw new BusinessException("No options provided.");

        // Tüm seçeneklerin aynı soruya ait olduğunu kontrol et
        var questionId = requests.First().QuestionId;
        if (requests.Any(r => r.QuestionId != questionId))
            throw new BusinessException("All options must belong to the same question.");

        await CheckQuizOwnership(questionId, userId);

        var options = _mapper.Map<List<Option>>(requests);
        var result = await _optionWriteRepository.AddRangeAsync(options);
        if (!result)
            throw new BusinessException("Failed to create options");

        return _mapper.Map<List<OptionDTO>>(options);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var option = await _optionReadRepository.GetByIdAsync(id);
        if (option == null)
            throw new NotFoundException($"Option with ID {id} not found.");

        await CheckQuizOwnership(option.QuestionId, userId);

        var result = await _optionWriteRepository.RemoveById(id);
        if (!result)
            throw new BusinessException("Failed to delete option");

        return result;
    }

    public async Task<bool> DeleteRange(List<Guid> ids, Guid userId)
    {
        var options = _optionReadRepository.GetWhere(x => ids.Contains(x.Id)).ToList();
        if (!options.Any())
            throw new NotFoundException("No options found with the provided IDs.");

        // Tüm seçeneklerin aynı soruya ait olduğunu kontrol et
        var questionId = options.First().QuestionId;
        if (options.Any(o => o.QuestionId != questionId))
            throw new BusinessException("All options must belong to the same question.");

        await CheckQuizOwnership(questionId, userId);

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

    public async Task<bool> Update(UpdateOptionRequest request, Guid userId)
    {
        var option = await _optionReadRepository.GetByIdAsync(request.Id);
        if (option == null)
            throw new NotFoundException($"Option with ID {request.Id} not found.");

        await CheckQuizOwnership(option.QuestionId, userId);

        _mapper.Map(request, option);
        var result = _optionWriteRepository.Update(option);
        if (!result)
            throw new BusinessException($"Failed to update option with ID {request.Id}");

        return result;
    }
}