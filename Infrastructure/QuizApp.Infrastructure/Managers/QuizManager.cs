using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Requests.QuizQuestion;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.DTOs.Responses.QuizQuestion;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.Quiz;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Repositories.Quiz;

namespace QuizApp.Infrastructure.Managers;

public class QuizManager : IQuizService
{
    private readonly IQuizReadRepository _quizReadRepository;
    private readonly IQuizWriteRepository _quizWriteRepository;
    private readonly IQuizQuestionService _quizQuestionService;
    private readonly IMapper _mapper;

    public QuizManager(
        IQuizReadRepository quizReadRepository,
        IQuizWriteRepository quizWriteRepository,
        IQuizQuestionService quizQuestionService,
        IMapper mapper)
    {
        _quizReadRepository = quizReadRepository;
        _quizWriteRepository = quizWriteRepository;
        _quizQuestionService = quizQuestionService;
        _mapper = mapper;
    }

    public async Task<QuizResponse> CreateAsync(CreateQuizRequest request)
    {
        var quiz = _mapper.Map<Quiz>(request);
        await _quizWriteRepository.AddAsync(quiz);
        await _quizWriteRepository.SaveAsync();
        return _mapper.Map<QuizResponse>(quiz);
    }

    public async Task<QuizResponse> UpdateAsync(UpdateQuizRequest request)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(request.Id);
        if (quiz == null)
            throw new NotFoundException($"Quiz with ID {request.Id} not found");

        _mapper.Map(request, quiz);
        _quizWriteRepository.Update(quiz);
        await _quizWriteRepository.SaveAsync();
        return _mapper.Map<QuizResponse>(quiz);
    }

    public async Task DeleteAsync(DeleteQuizRequest request)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(request.Id);
        if (quiz == null)
            throw new NotFoundException($"Quiz with ID {request.Id} not found");

        _quizWriteRepository.Remove(quiz);
        await _quizWriteRepository.SaveAsync();
    }

    public async Task<QuizResponse> GetByIdAsync(GetQuizByIdRequest request)
    {
        var quiz = await _quizReadRepository.GetByIdWithQuestionsAsync(request.Id);
        if (quiz == null)
            throw new NotFoundException($"Quiz with ID {request.Id} not found");

        return _mapper.Map<QuizResponse>(quiz);
    }

    public async Task<IEnumerable<QuizResponse>> GetAllAsync(GetQuizzesRequest request)
    {
        var query = _quizReadRepository.GetAll()
            .Include(q => q.Category)
            .Include(q => q.Creator)
            .Include(q => q.QuizQuestions)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchText))
            query = query.Where(q => q.Title.Contains(request.SearchText));

        if (request.CategoryId.HasValue)
            query = query.Where(q => q.CategoryId == request.CategoryId);

        if (request.CreatorId.HasValue)
            query = query.Where(q => q.CreatorId == request.CreatorId);

        if (request.IsActive)
            query = query.Where(q => q.QuizQuestions.Any(qq => qq.Question.IsActive));

        var quizzes = await query.ToListAsync();
        return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
    }

    public async Task<IEnumerable<QuizResponse>> GetByCategoryAsync(GetQuizzesByCategoryRequest request)
    {
        var quizzes = await _quizReadRepository.GetAll()
            .Include(q => q.Category)
            .Include(q => q.Creator)
            .Where(q => q.CategoryId == request.CategoryId && q.QuizQuestions.Any(qq => qq.Question.IsActive))
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
    }

    public async Task<IEnumerable<QuizResponse>> GetByUserAsync(GetQuizzesByUserRequest request)
    {
        var quizzes = await _quizReadRepository.GetAllByCreator(request.UserId)
            .Include(q => q.Category)
            .Include(q => q.Creator)
            .Where(q => q.QuizQuestions.Any(qq => qq.Question.IsActive))
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
    }

    public async Task<IEnumerable<QuizResponse>> GetActiveAsync(GetActiveQuizzesRequest request)
    {
        var quizzes = await _quizReadRepository.GetAll()
            .Include(q => q.Category)
            .Include(q => q.Creator)
            .Where(q => q.QuizQuestions.Any(qq => qq.Question.IsActive))
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
    }

    public async Task<IEnumerable<QuizResponse>> CreateRangeAsync(CreateRangeQuizRequest request)
    {
        var quizzes = _mapper.Map<IEnumerable<Quiz>>(request.Quizzes);
        await _quizWriteRepository.AddRangeAsync(quizzes.ToList());
        await _quizWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
    }

    public async Task<IEnumerable<QuizResponse>> UpdateRangeAsync(UpdateRangeQuizRequest request)
    {
        var quizzes = await _quizReadRepository.GetAll()
            .Where(q => request.Quizzes.Select(r => r.Id).Contains(q.Id))
            .ToListAsync();

        if (quizzes.Count != request.Quizzes.Count)
            throw new NotFoundException("One or more quizzes not found");

        foreach (var quiz in quizzes)
        {
            var updateRequest = request.Quizzes.FirstOrDefault(q => q.Id == quiz.Id);
            if (updateRequest != null)
                _mapper.Map(updateRequest, quiz);
        }

        _quizWriteRepository.UpdateRange(quizzes);
        await _quizWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
    }

    public async Task DeleteRangeAsync(DeleteRangeQuizRequest request)
    {
        var quizzes = await _quizReadRepository.GetWhere(x => request.Ids.Select(r => r).Contains(x.Id)).ToListAsync();
        if (quizzes.Count != request.Ids.Count)
            throw new NotFoundException("One or more quizzes not found");

        _quizWriteRepository.RemoveRange(quizzes.ToList());
        await _quizWriteRepository.SaveAsync();
    }
}