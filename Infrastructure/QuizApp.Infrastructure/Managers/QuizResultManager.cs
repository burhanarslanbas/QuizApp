using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Requests.QuizResult.Read;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class QuizResultManager : IQuizResultService
{
    private readonly IQuizResultWriteRepository _quizResultWriteRepository;
    private readonly IQuizResultReadRepository _quizResultReadRepository;
    private readonly IMapper _mapper;

    public QuizResultManager(IQuizResultWriteRepository quizResultWriteRepository, IQuizResultReadRepository quizResultReadRepository, IMapper mapper)
    {
        _quizResultWriteRepository = quizResultWriteRepository;
        _quizResultReadRepository = quizResultReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateQuizResultRequest request)
    {
        var quizResult = _mapper.Map<QuizResult>(request);
        return await _quizResultWriteRepository.AddAsync(quizResult);
    }

    public Task<List<QuizResultDTO>> CreateRangeAsync(List<CreateQuizResultRequest> requests)
    {
        var quizResults = _mapper.Map<List<QuizResult>>(requests);
        return _quizResultWriteRepository.AddRangeAsync(quizResults)
            .ContinueWith(task => task.Result ? _mapper.Map<List<QuizResultDTO>>(quizResults) : null);
    }

    public Task<bool> Delete(Guid id)
    {
        return _quizResultWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var quizResults = _quizResultReadRepository.GetWhere(qr => ids.Contains(qr.Id)).ToList();
        if (quizResults == null || !quizResults.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_quizResultWriteRepository.RemoveRange(quizResults));
    }

    public Task<List<QuizResultDTO>> GetAll()
    {
        var quizResults = _quizResultReadRepository.GetAll();
        if (quizResults == null || !quizResults.Any())
        {
            return Task.FromResult(new List<QuizResultDTO>());
        }
        return Task.FromResult(_mapper.Map<List<QuizResultDTO>>(quizResults));
    }

    public Task<QuizResultDTO> GetById(Guid id)
    {
        var quizResult = _quizResultReadRepository.GetByIdAsync(id).Result;
        if (quizResult == null)
        {
            return Task.FromResult<QuizResultDTO>(null);
        }
        return Task.FromResult(_mapper.Map<QuizResultDTO>(quizResult));
    }

    public Task<QuizResultDTO> Update(UpdateQuizResultRequest request)
    {
        var quizResult = _mapper.Map<QuizResult>(request);
        if (_quizResultWriteRepository.Update(quizResult))
        {
            return Task.FromResult(_mapper.Map<QuizResultDTO>(quizResult));
        }
        throw new Exception("QuizResult update failed.");
    }

    public Task<List<QuizResultDTO>> GetByQuizId(Guid quizId)
    {
        var quizResults = _quizResultReadRepository.GetWhere(qr => qr.QuizId == quizId).ToList();
        return Task.FromResult(_mapper.Map<List<QuizResultDTO>>(quizResults));
    }

    public Task<List<QuizResultDTO>> GetByStudentId(Guid studentId)
    {
        var quizResults = _quizResultReadRepository.GetWhere(qr => qr.StudentId == studentId).ToList();
        return Task.FromResult(_mapper.Map<List<QuizResultDTO>>(quizResults));
    }

    public async Task<QuizResultDTO> GetLatestResult(Guid quizId, Guid studentId)
    {
        var quizResult = await _quizResultReadRepository.GetWhere(qr => 
            qr.QuizId == quizId && 
            qr.StudentId == studentId)
            .OrderByDescending(qr => qr.CreatedDate)
            .FirstOrDefaultAsync();
        
        return quizResult != null ? _mapper.Map<QuizResultDTO>(quizResult) : null;
    }

    public async Task<int> GetAttemptCount(Guid quizId, Guid studentId)
    {
        return await _quizResultReadRepository.GetWhere(qr => 
            qr.QuizId == quizId && 
            qr.StudentId == studentId)
            .CountAsync();
    }

    public async Task<bool> CompleteQuiz(Guid quizResultId, int score)
    {
        var quizResult = await _quizResultReadRepository.GetByIdAsync(quizResultId);
        if (quizResult == null)
            return false;

        quizResult.Score = score;
        quizResult.IsCompleted = true;
        return _quizResultWriteRepository.Update(quizResult);
    }

    public async Task<Dictionary<Guid, int>> GetQuizScores(Guid quizId)
    {
        var quizResults = await _quizResultReadRepository.GetWhere(qr => 
            qr.QuizId == quizId && 
            qr.IsCompleted)
            .ToListAsync();

        return quizResults.ToDictionary(qr => qr.StudentId, qr => qr.Score);
    }

    public async Task<double> GetAverageScore(Guid quizId)
    {
        var quizResults = await _quizResultReadRepository.GetWhere(qr => 
            qr.QuizId == quizId && 
            qr.IsCompleted)
            .ToListAsync();

        if (!quizResults.Any())
            return 0;

        return quizResults.Average(qr => qr.Score);
    }

    public async Task<List<QuizResultDTO>> GetTopScores(Guid quizId, int count)
    {
        var quizResults = await _quizResultReadRepository.GetWhere(qr => 
            qr.QuizId == quizId && 
            qr.IsCompleted)
            .OrderByDescending(qr => qr.Score)
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<QuizResultDTO>>(quizResults);
    }
} 