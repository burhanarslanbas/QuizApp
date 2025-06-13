using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.UserAnswer;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class UserAnswerManager : IUserAnswerService
{
    private readonly IUserAnswerReadRepository _userAnswerReadRepository;
    private readonly IUserAnswerWriteRepository _userAnswerWriteRepository;
    private readonly IMapper _mapper;

    public UserAnswerManager(IUserAnswerReadRepository userAnswerReadRepository, IUserAnswerWriteRepository userAnswerWriteRepository, IMapper mapper)
    {
        _userAnswerReadRepository = userAnswerReadRepository;
        _userAnswerWriteRepository = userAnswerWriteRepository;
        _mapper = mapper;
    }

    public async Task DeleteAsync(DeleteUserAnswerRequest request)
    {
        var entity = await _userAnswerReadRepository.GetByIdAsync(request.Id);
        var result = _userAnswerWriteRepository.Remove(entity);
        if (!result)
            throw new NotFoundException($"User answer with ID {request.Id} not found.");
    }

    public async Task DeleteRangeAsync(DeleteRangeUserAnswerRequest request)
    {
        var userAnswers = await _userAnswerReadRepository.GetWhere(x => request.Ids.Select(r => r).Contains(x.Id)).ToListAsync();
        if (!userAnswers.Any())
            throw new NotFoundException("No user answers found with the provided IDs.");

        _userAnswerWriteRepository.RemoveRange(userAnswers.ToList());
        await _userAnswerWriteRepository.SaveAsync();
    }

    public async Task<IEnumerable<UserAnswerResponse>> GetAllAsync(GetUserAnswersRequest request)
    {
        var userAnswers = await _userAnswerReadRepository.GetAll().ToListAsync();
        return _mapper.Map<IEnumerable<UserAnswerResponse>>(userAnswers);
    }

    public async Task<UserAnswerResponse> GetByIdAsync(GetUserAnswerByIdRequest request)
    {
        try
        {
            var userAnswer = await _userAnswerReadRepository.GetByIdAsync(request.Id);
            return _mapper.Map<UserAnswerResponse>(userAnswer);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"User answer with ID {request.Id} not found.");
        }
    }

    public async Task<IEnumerable<UserAnswerResponse>> GetByQuizResultAsync(GetUserAnswersByQuizResultRequest request)
    {
        var userAnswers = await _userAnswerReadRepository.GetWhere(x => x.QuizResultId == request.QuizResultId).ToListAsync();
        return _mapper.Map<IEnumerable<UserAnswerResponse>>(userAnswers);
    }

    public async Task<UserAnswerResponse> UpdateAsync(UpdateUserAnswerRequest request)
    {
        try
        {
            var userAnswer = await _userAnswerReadRepository.GetByIdAsync(request.Id);
            _mapper.Map(request, userAnswer);
            var result = _userAnswerWriteRepository.Update(userAnswer);
            if (!result)
                throw new BusinessException($"Failed to update user answer with ID {request.Id}");

            return _mapper.Map<UserAnswerResponse>(userAnswer);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"User answer with ID {request.Id} not found.");
        }
    }

    public async Task<UserAnswerResponse> CreateAsync(CreateUserAnswerRequest request)
    {
        var userAnswer = _mapper.Map<UserAnswer>(request);
        await _userAnswerWriteRepository.AddAsync(userAnswer);
        await _userAnswerWriteRepository.SaveAsync();
        return _mapper.Map<UserAnswerResponse>(userAnswer);
    }
}