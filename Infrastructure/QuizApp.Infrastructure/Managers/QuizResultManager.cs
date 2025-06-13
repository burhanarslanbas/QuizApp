using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.QuizResult;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class QuizResultManager : IQuizResultService
{
    private readonly IQuizResultReadRepository _quizResultReadRepository;
    private readonly IQuizResultWriteRepository _quizResultWriteRepository;
    private readonly IMapper _mapper;

    public QuizResultManager(
        IQuizResultReadRepository quizResultReadRepository,
        IQuizResultWriteRepository quizResultWriteRepository,
        IMapper mapper)
    {
        _quizResultReadRepository = quizResultReadRepository;
        _quizResultWriteRepository = quizResultWriteRepository;
        _mapper = mapper;
    }

    public async Task<QuizResultResponse> CreateAsync(CreateQuizResultRequest request)
    {
        var quizResult = _mapper.Map<QuizResult>(request);
        await _quizResultWriteRepository.AddAsync(quizResult);
        await _quizResultWriteRepository.SaveAsync();
        return _mapper.Map<QuizResultResponse>(quizResult);
    }

    public async Task<QuizResultResponse> UpdateAsync(UpdateQuizResultRequest request)
    {
        var quizResult = await _quizResultReadRepository.GetByIdAsync(request.Id);
        if (quizResult == null)
            throw new NotFoundException($"Quiz result with ID {request.Id} not found");

        _mapper.Map(request, quizResult);
        _quizResultWriteRepository.Update(quizResult);
        await _quizResultWriteRepository.SaveAsync();
        return _mapper.Map<QuizResultResponse>(quizResult);
    }

    public async Task DeleteAsync(DeleteQuizResultRequest request)
    {
        var quizResult = await _quizResultReadRepository.GetByIdAsync(request.Id);
        if (quizResult == null)
            throw new NotFoundException($"Quiz result with ID {request.Id} not found");

        _quizResultWriteRepository.Remove(quizResult);
        await _quizResultWriteRepository.SaveAsync();
    }

    public async Task<QuizResultResponse> GetByIdAsync(GetQuizResultByIdRequest request)
    {
        var quizResult = await _quizResultReadRepository.GetAll()
            .Include(qr => qr.Quiz)
            .Include(qr => qr.User)
            .FirstOrDefaultAsync(qr => qr.Id == request.Id);

        if (quizResult == null)
            throw new NotFoundException($"Quiz result with ID {request.Id} not found");

        return _mapper.Map<QuizResultResponse>(quizResult);
    }

    public async Task<IEnumerable<QuizResultResponse>> GetAllAsync(GetQuizResultsRequest request)
    {
        var query = _quizResultReadRepository.GetAll()
            .Include(qr => qr.Quiz)
            .Include(qr => qr.User)
            .AsQueryable();

        if (request.QuizId.HasValue)
            query = query.Where(qr => qr.QuizId == request.QuizId);

        if (request.UserId.HasValue)
            query = query.Where(qr => qr.UserId == request.UserId);

        var quizResults = await query.ToListAsync();
        return _mapper.Map<IEnumerable<QuizResultResponse>>(quizResults);
    }

    public async Task<IEnumerable<QuizResultResponse>> GetByUserAsync(GetQuizResultsByUserRequest request)
    {
        var quizResults = await _quizResultReadRepository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<QuizResultResponse>>(quizResults);
    }

    public async Task<IEnumerable<QuizResultResponse>> GetByQuizAsync(GetQuizResultsByQuizRequest request)
    {
        var quizResults = await _quizResultReadRepository.GetByQuizIdAsync(request.QuizId);
        return _mapper.Map<IEnumerable<QuizResultResponse>>(quizResults);
    }

    public async Task<IEnumerable<QuizResultResponse>> CreateRangeAsync(CreateRangeQuizResultRequest request)
    {
        var quizResults = _mapper.Map<IEnumerable<QuizResult>>(request.QuizResults);
        await _quizResultWriteRepository.AddRangeAsync(quizResults.ToList());
        await _quizResultWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<QuizResultResponse>>(quizResults);
    }

    public async Task<IEnumerable<QuizResultResponse>> UpdateRangeAsync(UpdateRangeQuizResultRequest request)
    {
        var quizResults = await _quizResultReadRepository.GetAll()
            .Where(qr => request.Ids.Contains(qr.Id))
            .ToListAsync();

        if (quizResults.Count != request.Ids.Count)
            throw new NotFoundException("One or more quiz results not found");

        foreach (var quizResult in quizResults)
        {
            var updateRequest = request.QuizResults.FirstOrDefault(qr => qr.Id == quizResult.Id);
            if (updateRequest != null)
                _mapper.Map(updateRequest, quizResult);
        }

        _quizResultWriteRepository.UpdateRange(quizResults);
        await _quizResultWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<QuizResultResponse>>(quizResults);
    }

    public async Task DeleteRangeAsync(DeleteRangeQuizResultRequest request)
    {
        var quizResults = await _quizResultReadRepository.GetAll()
            .Where(qr => request.Ids.Contains(qr.Id))
            .ToListAsync();

        if (quizResults.Count != request.Ids.Count)
            throw new NotFoundException("One or more quiz results not found");

        _quizResultWriteRepository.RemoveRange(quizResults);
        await _quizResultWriteRepository.SaveAsync();
    }
}