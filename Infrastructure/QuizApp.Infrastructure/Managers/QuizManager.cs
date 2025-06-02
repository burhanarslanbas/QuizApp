using AutoMapper;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Infrastructure.Managers;

public class QuizManager : IQuizService
{
    private readonly IQuizReadRepository _quizReadRepository;
    private readonly IQuizWriteRepository _quizWriteRepository;
    private readonly IMapper _mapper;

    public QuizManager(IQuizReadRepository quizReadRepository, IQuizWriteRepository quizWriteRepository, IMapper mapper)
    {
        _quizReadRepository = quizReadRepository;
        _quizWriteRepository = quizWriteRepository;
        _mapper = mapper;
    }

    public async Task<QuizDetailResponse> CreateAsync(CreateQuizRequest request)
    {
        var quiz = _mapper.Map<Quiz>(request);
        var result = await _quizWriteRepository.AddAsync(quiz);
        if (!result)
            throw new BusinessException("Failed to create quiz");

        return _mapper.Map<QuizDetailResponse>(quiz);
    }

    public async Task DeleteAsync(DeleteQuizRequest request)
    {
        var result = await _quizWriteRepository.RemoveById(request.Id);
        if (!result)
            throw new NotFoundException($"Quiz with ID {request.Id} not found.");
    }

    public bool DeleteRange(DeleteRangeQuizRequest request)
    {
        var quizzes = _quizReadRepository.GetWhere(x => request.Ids.Contains(x.Id)).ToList();
        if (!quizzes.Any())
            throw new NotFoundException("No quizzes found with the provided IDs.");

        var result = _quizWriteRepository.RemoveRange(quizzes);
        if (!result)
            throw new BusinessException("Failed to delete quizzes.");

        return result;
    }

    public List<QuizDetailResponse> GetAll(GetQuizzesRequest request)
    {
        var quizzes = _quizReadRepository.GetAll();
        if (request.CreatorId.HasValue)
        {
            quizzes = quizzes.Where(q => q.CreatorId == request.CreatorId.Value);
        }
        // Diğer filtreler (kategori, aktiflik vs.) burada eklenebilir
        return _mapper.Map<List<QuizDetailResponse>>(quizzes.ToList());
    }

    public async Task<QuizDetailResponse> GetByIdAsync(GetQuizByIdRequest request)
    {
        try
        {
            var quiz = await _quizReadRepository.GetByIdAsync(request.Id);
            return _mapper.Map<QuizDetailResponse>(quiz);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Quiz with ID {request.Id} not found.");
        }
    }

    public QuizDetailResponse Update(UpdateQuizRequest request)
    {
        try
        {
            var quiz = _quizReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, quiz);
            var result = _quizWriteRepository.Update(quiz);
            if (!result)
                throw new BusinessException($"Failed to update quiz with ID {request.Id}");

            return _mapper.Map<QuizDetailResponse>(quiz);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Quiz with ID {request.Id} not found.");
        }
    }
}