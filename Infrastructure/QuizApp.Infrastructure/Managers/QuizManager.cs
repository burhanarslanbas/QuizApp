using AutoMapper;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Requests.Quiz.Read;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class QuizManager : IQuizService
{
    private readonly IQuizWriteRepository _quizWriteRepository;
    private readonly IQuizReadRepository _quizReadRepository;
    private readonly IMapper _mapper;

    public QuizManager(IQuizWriteRepository quizWriteRepository, IQuizReadRepository quizReadRepository, IMapper mapper)
    {
        _quizWriteRepository = quizWriteRepository;
        _quizReadRepository = quizReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateQuizRequest request)
    {
        var quiz = _mapper.Map<Quiz>(request);
        return await _quizWriteRepository.AddAsync(quiz);
    }

    public Task<List<QuizDTO>> CreateRangeAsync(List<CreateQuizRequest> requests)
    {
        var quizzes = _mapper.Map<List<Quiz>>(requests);
        return _quizWriteRepository.AddRangeAsync(quizzes)
            .ContinueWith(task => task.Result ? _mapper.Map<List<QuizDTO>>(quizzes) : null);
    }

    public Task<bool> Delete(Guid id)
    {
        return _quizWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var quizzes = _quizReadRepository.GetWhere(q => ids.Contains(q.Id)).ToList();
        if (quizzes == null || !quizzes.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_quizWriteRepository.RemoveRange(quizzes));
    }

    public Task<List<QuizDTO>> GetAll()
    {
        var quizzes = _quizReadRepository.GetAll();
        if (quizzes == null || !quizzes.Any())
        {
            return Task.FromResult(new List<QuizDTO>());
        }
        return Task.FromResult(_mapper.Map<List<QuizDTO>>(quizzes));
    }

    public Task<QuizDTO> GetById(Guid id)
    {
        var quiz = _quizReadRepository.GetByIdAsync(id).Result;
        if (quiz == null)
        {
            return Task.FromResult<QuizDTO>(null);
        }
        return Task.FromResult(_mapper.Map<QuizDTO>(quiz));
    }

    public Task<QuizDTO> Update(UpdateQuizRequest request)
    {
        var quiz = _mapper.Map<Quiz>(request);
        if (_quizWriteRepository.Update(quiz))
        {
            return Task.FromResult(_mapper.Map<QuizDTO>(quiz));
        }
        throw new Exception("Quiz update failed.");
    }

    public Task<List<QuizDTO>> GetByCategoryId(Guid categoryId)
    {
        var quizzes = _quizReadRepository.GetWhere(q => q.CategoryId == categoryId).ToList();
        return Task.FromResult(_mapper.Map<List<QuizDTO>>(quizzes));
    }

    public Task<List<QuizDTO>> GetByCreatorId(Guid creatorId)
    {
        var quizzes = _quizReadRepository.GetWhere(q => q.CreatorId == creatorId).ToList();
        return Task.FromResult(_mapper.Map<List<QuizDTO>>(quizzes));
    }

    public async Task<bool> IsQuizAvailable(Guid quizId)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(quizId);
        if (quiz == null || !quiz.IsActive)
            return false;

        var now = DateTime.UtcNow;
        if (quiz.StartDate.HasValue && quiz.StartDate.Value > now)
            return false;

        if (quiz.EndDate.HasValue && quiz.EndDate.Value < now)
            return false;

        return true;
    }

    public async Task<bool> CanUserAttemptQuiz(Guid quizId, Guid userId)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(quizId);
        if (quiz == null || !quiz.IsActive)
            return false;

        // TODO: Implement attempt count check
        // var attemptCount = await _quizResultRepository.GetAttemptCount(quizId, userId);
        // return attemptCount < quiz.MaxAttempts;

        return true;
    }

    public async Task<int> GetRemainingAttempts(Guid quizId, Guid userId)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(quizId);
        if (quiz == null)
            return 0;

        // TODO: Implement attempt count check
        // var attemptCount = await _quizResultRepository.GetAttemptCount(quizId, userId);
        // return Math.Max(0, quiz.MaxAttempts - attemptCount);

        return quiz.MaxAttempts;
    }

    public async Task<bool> ActivateQuiz(Guid quizId)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(quizId);
        if (quiz == null)
            return false;

        quiz.IsActive = true;
        return _quizWriteRepository.Update(quiz);
    }

    public async Task<bool> DeactivateQuiz(Guid quizId)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(quizId);
        if (quiz == null)
            return false;

        quiz.IsActive = false;
        return _quizWriteRepository.Update(quiz);
    }

    public Task<List<QuizDTO>> GetActiveQuizzes()
    {
        var now = DateTime.UtcNow;
        var quizzes = _quizReadRepository.GetWhere(q => 
            q.IsActive && 
            (!q.StartDate.HasValue || q.StartDate.Value <= now) &&
            (!q.EndDate.HasValue || q.EndDate.Value >= now)
        ).ToList();
        return Task.FromResult(_mapper.Map<List<QuizDTO>>(quizzes));
    }

    public Task<List<QuizDTO>> GetUpcomingQuizzes()
    {
        var now = DateTime.UtcNow;
        var quizzes = _quizReadRepository.GetWhere(q => 
            q.IsActive && 
            q.StartDate.HasValue && 
            q.StartDate.Value > now
        ).ToList();
        return Task.FromResult(_mapper.Map<List<QuizDTO>>(quizzes));
    }

    public Task<List<QuizDTO>> GetExpiredQuizzes()
    {
        var now = DateTime.UtcNow;
        var quizzes = _quizReadRepository.GetWhere(q => 
            q.IsActive && 
            q.EndDate.HasValue && 
            q.EndDate.Value < now
        ).ToList();
        return Task.FromResult(_mapper.Map<List<QuizDTO>>(quizzes));
    }
} 