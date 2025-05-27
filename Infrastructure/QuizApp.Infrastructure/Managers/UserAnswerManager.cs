using AutoMapper;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Requests.UserAnswer.Read;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class UserAnswerManager : IUserAnswerService
{
    private readonly IUserAnswerWriteRepository _userAnswerWriteRepository;
    private readonly IUserAnswerReadRepository _userAnswerReadRepository;
    private readonly IQuestionReadRepository _questionReadRepository;
    private readonly IOptionReadRepository _optionReadRepository;
    private readonly IMapper _mapper;

    public UserAnswerManager(
        IUserAnswerWriteRepository userAnswerWriteRepository,
        IUserAnswerReadRepository userAnswerReadRepository,
        IQuestionReadRepository questionReadRepository,
        IOptionReadRepository optionReadRepository,
        IMapper mapper)
    {
        _userAnswerWriteRepository = userAnswerWriteRepository;
        _userAnswerReadRepository = userAnswerReadRepository;
        _questionReadRepository = questionReadRepository;
        _optionReadRepository = optionReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateUserAnswerRequest request)
    {
        var userAnswer = _mapper.Map<UserAnswer>(request);
        return await _userAnswerWriteRepository.AddAsync(userAnswer);
    }

    public Task<List<UserAnswerDTO>> CreateRangeAsync(List<CreateUserAnswerRequest> requests)
    {
        var userAnswers = _mapper.Map<List<UserAnswer>>(requests);
        return _userAnswerWriteRepository.AddRangeAsync(userAnswers)
            .ContinueWith(task => task.Result ? _mapper.Map<List<UserAnswerDTO>>(userAnswers) : null);
    }

    public Task<bool> Delete(Guid id)
    {
        return _userAnswerWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var userAnswers = _userAnswerReadRepository.GetWhere(ua => ids.Contains(ua.Id)).ToList();
        if (userAnswers == null || !userAnswers.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_userAnswerWriteRepository.RemoveRange(userAnswers));
    }

    public Task<List<UserAnswerDTO>> GetAll()
    {
        var userAnswers = _userAnswerReadRepository.GetAll();
        if (userAnswers == null || !userAnswers.Any())
        {
            return Task.FromResult(new List<UserAnswerDTO>());
        }
        return Task.FromResult(_mapper.Map<List<UserAnswerDTO>>(userAnswers));
    }

    public Task<UserAnswerDTO> GetById(Guid id)
    {
        var userAnswer = _userAnswerReadRepository.GetByIdAsync(id).Result;
        if (userAnswer == null)
        {
            return Task.FromResult<UserAnswerDTO>(null);
        }
        return Task.FromResult(_mapper.Map<UserAnswerDTO>(userAnswer));
    }

    public Task<UserAnswerDTO> Update(UpdateUserAnswerRequest request)
    {
        var userAnswer = _mapper.Map<UserAnswer>(request);
        if (_userAnswerWriteRepository.Update(userAnswer))
        {
            return Task.FromResult(_mapper.Map<UserAnswerDTO>(userAnswer));
        }
        throw new Exception("UserAnswer update failed.");
    }

    public Task<List<UserAnswerDTO>> GetByQuizResultId(Guid quizResultId)
    {
        var userAnswers = _userAnswerReadRepository.GetWhere(ua => ua.QuizResultId == quizResultId).ToList();
        return Task.FromResult(_mapper.Map<List<UserAnswerDTO>>(userAnswers));
    }

    public Task<List<UserAnswerDTO>> GetByQuestionId(Guid questionId)
    {
        var userAnswers = _userAnswerReadRepository.GetWhere(ua => ua.QuestionId == questionId).ToList();
        return Task.FromResult(_mapper.Map<List<UserAnswerDTO>>(userAnswers));
    }

    public async Task<bool> ValidateAnswer(Guid userAnswerId)
    {
        var userAnswer = await _userAnswerReadRepository.GetByIdAsync(userAnswerId);
        if (userAnswer == null)
            return false;

        var question = await _questionReadRepository.GetByIdAsync(userAnswer.QuestionId);
        if (question == null)
            return false;

        switch (question.QuestionType)
        {
            case QuestionType.SingleChoice:
            case QuestionType.MultipleChoice:
                if (!userAnswer.OptionId.HasValue)
                    return false;
                var option = await _optionReadRepository.GetByIdAsync(userAnswer.OptionId.Value);
                return option?.IsCorrect ?? false;

            case QuestionType.Text:
                // TODO: Implement text answer validation
                return false;

            default:
                return false;
        }
    }

    public async Task<int> CalculateScore(Guid quizResultId)
    {
        var userAnswers = await _userAnswerReadRepository.GetWhere(ua => ua.QuizResultId == quizResultId).ToListAsync();
        var totalScore = 0;

        foreach (var userAnswer in userAnswers)
        {
            if (userAnswer.IsCorrect)
            {
                var question = await _questionReadRepository.GetByIdAsync(userAnswer.QuestionId);
                if (question != null)
                {
                    totalScore += question.Points;
                }
            }
        }

        return totalScore;
    }

    public async Task<Dictionary<Guid, bool>> GetAnswerResults(Guid quizResultId)
    {
        var userAnswers = await _userAnswerReadRepository.GetWhere(ua => ua.QuizResultId == quizResultId).ToListAsync();
        return userAnswers.ToDictionary(ua => ua.QuestionId, ua => ua.IsCorrect);
    }
} 