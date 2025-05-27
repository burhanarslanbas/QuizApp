using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Infrastructure.Managers;

public class QuizResultManager : IQuizResultService
{
    private readonly IQuizResultReadRepository _quizResultReadRepository;
    private readonly IQuizResultWriteRepository _quizResultWriteRepository;
    private readonly IMapper _mapper;

    public QuizResultManager(IQuizResultReadRepository quizResultReadRepository, IQuizResultWriteRepository quizResultWriteRepository, IMapper mapper)
    {
        _quizResultReadRepository = quizResultReadRepository;
        _quizResultWriteRepository = quizResultWriteRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateQuizResultRequest request)
    {
        var quizResult = _mapper.Map<QuizResult>(request);
        var result = await _quizResultWriteRepository.AddAsync(quizResult);
        if (!result)
            throw new BusinessException("Failed to create quiz result");

        return result;
    }

    public async Task<List<QuizResultDTO>> CreateRangeAsync(List<CreateQuizResultRequest> requests)
    {
        var quizResults = _mapper.Map<List<QuizResult>>(requests);
        var result = await _quizResultWriteRepository.AddRangeAsync(quizResults);
        if (!result)
            throw new BusinessException("Failed to create quiz results");

        return _mapper.Map<List<QuizResultDTO>>(quizResults);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _quizResultWriteRepository.RemoveById(id);
        if (!result)
            throw new NotFoundException($"Quiz result with ID {id} not found.");

        return result;
    }

    public bool DeleteRange(List<Guid> ids)
    {
        var quizResults = _quizResultReadRepository.GetWhere(x => ids.Contains(x.Id)).ToList();
        if (!quizResults.Any())
            throw new NotFoundException("No quiz results found with the provided IDs.");

        var result = _quizResultWriteRepository.RemoveRange(quizResults);
        if (!result)
            throw new BusinessException("Failed to delete quiz results.");

        return result;
    }

    public List<QuizResultDTO> GetAll()
    {
        var quizResults = _quizResultReadRepository.GetAll().ToList();
        return _mapper.Map<List<QuizResultDTO>>(quizResults);
    }

    public async Task<QuizResultDTO> GetByIdAsync(Guid id)
    {
        try
        {
            var quizResult = await _quizResultReadRepository.GetByIdAsync(id);
            return _mapper.Map<QuizResultDTO>(quizResult);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Quiz result with ID {id} not found.");
        }
    }

    public bool Update(UpdateQuizResultRequest request)
    {
        try
        {
            var quizResult = _quizResultReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, quizResult);
            var result = _quizResultWriteRepository.Update(quizResult);
            if (!result)
                throw new BusinessException($"Failed to update quiz result with ID {request.Id}");

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Quiz result with ID {request.Id} not found.");
        }
    }
}