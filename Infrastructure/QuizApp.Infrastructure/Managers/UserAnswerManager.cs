using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories;
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

    public async Task<UserAnswerDetailResponse> CreateAsync(CreateUserAnswerRequest request)
    {
        var userAnswer = _mapper.Map<UserAnswer>(request);
        var result = await _userAnswerWriteRepository.AddAsync(userAnswer);
        if (!result)
            throw new BusinessException("Failed to create user answer");

        return _mapper.Map<UserAnswerDetailResponse>(userAnswer);
    }

    public async Task DeleteAsync(DeleteUserAnswerRequest request)
    {
        var result = await _userAnswerWriteRepository.RemoveById(request.Id);
        if (!result)
            throw new NotFoundException($"User answer with ID {request.Id} not found.");
    }

    public bool DeleteRange(DeleteRangeUserAnswerRequest request)
    {
        var userAnswers = _userAnswerReadRepository.GetWhere(x => request.Ids.Contains(x.Id)).ToList();
        if (!userAnswers.Any())
            throw new NotFoundException("No user answers found with the provided IDs.");

        var result = _userAnswerWriteRepository.RemoveRange(userAnswers);
        if (!result)
            throw new BusinessException("Failed to delete user answers.");

        return result;
    }

    public List<UserAnswerDetailResponse> GetAll(GetUserAnswersRequest request)
    {
        var userAnswers = _userAnswerReadRepository.GetAll().ToList();
        return _mapper.Map<List<UserAnswerDetailResponse>>(userAnswers);
    }

    public async Task<UserAnswerDetailResponse> GetByIdAsync(GetUserAnswerByIdRequest request)
    {
        try
        {
            var userAnswer = await _userAnswerReadRepository.GetByIdAsync(request.Id);
            return _mapper.Map<UserAnswerDetailResponse>(userAnswer);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"User answer with ID {request.Id} not found.");
        }
    }

    public List<UserAnswerDetailResponse> GetByQuizResult(GetUserAnswersByQuizResultRequest request)
    {
        var userAnswers = _userAnswerReadRepository.GetWhere(x => x.QuizResultId == request.QuizResultId).ToList();
        return _mapper.Map<List<UserAnswerDetailResponse>>(userAnswers);
    }

    public UserAnswerDetailResponse Update(UpdateUserAnswerRequest request)
    {
        try
        {
            var userAnswer = _userAnswerReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, userAnswer);
            var result = _userAnswerWriteRepository.Update(userAnswer);
            if (!result)
                throw new BusinessException($"Failed to update user answer with ID {request.Id}");

            return _mapper.Map<UserAnswerDetailResponse>(userAnswer);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"User answer with ID {request.Id} not found.");
        }
    }
}