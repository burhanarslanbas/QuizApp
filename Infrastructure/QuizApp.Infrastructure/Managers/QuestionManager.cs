using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.Question;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class QuestionManager : IQuestionService
{
    private readonly IQuestionReadRepository _questionReadRepository;
    private readonly IQuestionWriteRepository _questionWriteRepository;
    private readonly IMapper _mapper;

    public QuestionManager(
        IQuestionReadRepository questionReadRepository,
        IQuestionWriteRepository questionWriteRepository,
        IMapper mapper)
    {
        _questionReadRepository = questionReadRepository;
        _questionWriteRepository = questionWriteRepository;
        _mapper = mapper;
    }

    public async Task<QuestionResponse> CreateAsync(CreateQuestionRequest request)
    {
        var question = _mapper.Map<Question>(request);
        await _questionWriteRepository.AddAsync(question);
        await _questionWriteRepository.SaveAsync();
        return _mapper.Map<QuestionResponse>(question);
    }

    public async Task<QuestionResponse> UpdateAsync(UpdateQuestionRequest request)
    {
        var question = await _questionReadRepository.GetByIdAsync(request.Id);
        if (question == null)
            throw new NotFoundException($"Question with ID {request.Id} not found");

        _mapper.Map(request, question);
        _questionWriteRepository.Update(question);
        await _questionWriteRepository.SaveAsync();
        return _mapper.Map<QuestionResponse>(question);
    }

    public async Task DeleteAsync(DeleteQuestionRequest request)
    {
        var question = await _questionReadRepository.GetByIdAsync(request.Id);
        if (question == null)
            throw new NotFoundException($"Question with ID {request.Id} not found");

        _questionWriteRepository.Remove(question);
        await _questionWriteRepository.SaveAsync();
    }

    public async Task<QuestionResponse> GetByIdAsync(GetQuestionByIdRequest request)
    {
        var question = await _questionReadRepository.GetAll()
            .Include(q => q.Options)
            .Include(q => q.QuestionRepo)
            .FirstOrDefaultAsync(q => q.Id == request.Id);

        if (question == null)
            throw new NotFoundException($"Question with ID {request.Id} not found");

        return _mapper.Map<QuestionResponse>(question);
    }

    public async Task<IEnumerable<QuestionResponse>> GetAllAsync(GetQuestionsRequest request)
    {
        var query = _questionReadRepository.GetAll()
            .Include(q => q.Options)
            .Include(q => q.QuestionRepo)
            .Include(q => q.QuizQuestions)
            .AsQueryable();

        // Burada request quizId ve questionType ile filtreleme yapıyoruz.
        if (request.QuizId.HasValue)
            query = query.Where(q => q.QuizQuestions.Any(qq => qq.QuizId == request.QuizId));

        if (!string.IsNullOrEmpty(request.QuestionType.ToString()))
            query = query.Where(q => q.QuestionType == request.QuestionType);

        if (request.IsActive)
            query = query.Where(q => q.IsActive == request.IsActive);

        var questions = await query.ToListAsync();
        return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<IEnumerable<QuestionResponse>> GetByRepoAsync(GetQuestionsByRepoRequest request)
    {
        var questions = await _questionReadRepository.GetAll()
            .Include(q => q.Options)
            .Include(q => q.QuestionRepo)
            .Where(q => q.QuestionRepoId == request.QuestionRepoId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<IEnumerable<QuestionResponse>> CreateRangeAsync(CreateRangeQuestionRequest request)
    {
        var questions = _mapper.Map<IEnumerable<Question>>(request.Questions);
        await _questionWriteRepository.AddRangeAsync(questions.ToList());
        await _questionWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<IEnumerable<QuestionResponse>> UpdateRangeAsync(UpdateRangeQuestionRequest request)
    {
        var questions = await _questionReadRepository.GetAll()
            .Where(q => request.Questions.Select(r => r.Id).Contains(q.Id))
            .ToListAsync();

        if (questions.Count != request.Questions.Count)
            throw new NotFoundException("One or more questions not found");

        foreach (var question in questions)
        {
            var updateRequest = request.Questions.FirstOrDefault(q => q.Id == question.Id);
            if (updateRequest != null)
                _mapper.Map(updateRequest, question);
        }

        _questionWriteRepository.UpdateRange(questions);
        await _questionWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task DeleteRangeAsync(DeleteRangeQuestionRequest request)
    {
        var questions = await _questionReadRepository.GetAll()
            .Where(q => request.Ids.Contains(q.Id))
            .ToListAsync();

        if (questions.Count != request.Ids.Count)
            throw new NotFoundException("One or more questions not found");

        _questionWriteRepository.RemoveRange(questions);
        await _questionWriteRepository.SaveAsync();
    }

    public async Task UpdateRepoIdAsync(UpdateQuestionRepoIdRequest request)
    {
        var question = await _questionReadRepository.GetByIdAsync(request.Id);
        if (question == null)
            throw new NotFoundException($"Question with ID {request.Id} not found");

        question.QuestionRepoId = request.RepoId;
        _questionWriteRepository.Update(question);
        await _questionWriteRepository.SaveAsync();
    }

    public async Task UpdateRepoIdsAsync(UpdateQuestionRepoIdsRequest request)
    {
        var questions = await _questionReadRepository.GetAll()
            .Where(q => request.Requests.Select(r => r.Id).Contains(q.Id))
            .ToListAsync();

        if (questions.Count != request.Requests.Count)
            throw new NotFoundException("One or more questions not found");

        foreach (var question in questions)
        {
            var updateRequest = request.Requests.FirstOrDefault(r => r.Id == question.Id);
            if (updateRequest != null)
                question.QuestionRepoId = updateRequest.RepoId;
        }

        _questionWriteRepository.UpdateRange(questions);
        await _questionWriteRepository.SaveAsync();
    }

    public async Task<IEnumerable<QuestionResponse>> GetByCategoryAsync(GetQuestionsByCategoryRequest request)
    {
        var questions = await _questionReadRepository.GetAllWithDetails()
            .Include(q => q.QuizQuestions)
                .ThenInclude(qq => qq.Quiz)
            .Where(q => q.QuizQuestions.Any(qq => qq.Quiz.CategoryId == request.CategoryId) && q.IsActive)
            .ToListAsync();
        return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
    }
} 