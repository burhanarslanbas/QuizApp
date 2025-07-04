using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.UserAnswer;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Repositories.Question;
using QuizApp.Domain.Enums;

namespace QuizApp.Infrastructure.Managers;

public class UserAnswerManager : IUserAnswerService
{
    private readonly IUserAnswerReadRepository _userAnswerReadRepository;
    private readonly IUserAnswerWriteRepository _userAnswerWriteRepository;
    private readonly IQuestionReadRepository _questionReadRepository;
    private readonly IMapper _mapper;

    public UserAnswerManager(
        IUserAnswerReadRepository userAnswerReadRepository, 
        IUserAnswerWriteRepository userAnswerWriteRepository,
        IQuestionReadRepository questionReadRepository,
        IMapper mapper)
    {
        _userAnswerReadRepository = userAnswerReadRepository;
        _userAnswerWriteRepository = userAnswerWriteRepository;
        _questionReadRepository = questionReadRepository;
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
            // Get the existing user answer
            var userAnswer = await _userAnswerReadRepository.GetByIdAsync(request.Id);
            if (userAnswer == null)
                throw new NotFoundException($"User answer with ID {request.Id} not found.");

            // Get the question with its options
            var question = await _questionReadRepository.GetWhere(q => q.Id == request.QuestionId)
                .Include(q => q.Options)
                .FirstOrDefaultAsync();

            if (question == null)
                throw new NotFoundException($"Question with ID {request.QuestionId} not found.");

            if (!question.IsActive)
                throw new BusinessException($"Question with ID {request.QuestionId} is not active.");

            // Determine if the answer is correct based on question type
            bool isCorrect = false;

            switch (question.QuestionType)
            {
                case QuestionType.SingleChoice:
                    // For single choice questions, check if the selected option is marked as correct
                    var selectedOption = question.Options.FirstOrDefault(o => o.Id == request.OptionId);
                    if (selectedOption == null)
                        throw new BusinessException($"Selected option with ID {request.OptionId} not found for question.");
                    
                    isCorrect = selectedOption.IsCorrect;
                    break;

                case QuestionType.TrueFalse:
                    // For true/false questions, check if the selected option matches the correct answer
                    var tfSelectedOption = question.Options.FirstOrDefault(o => o.Id == request.OptionId);
                    if (tfSelectedOption == null)
                        throw new BusinessException($"Selected option with ID {request.OptionId} not found for question.");
                    
                    isCorrect = tfSelectedOption.IsCorrect;
                    break;

                case QuestionType.ShortAnswer:
                    // For short answer questions, compare with the correct option's text
                    var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                    if (correctOption == null)
                        throw new BusinessException("No correct option found for short answer question.");

                    isCorrect = !string.IsNullOrEmpty(request.TextAnswer) && 
                               !string.IsNullOrEmpty(correctOption.OptionText) &&
                               request.TextAnswer.Trim().Equals(correctOption.OptionText.Trim(), StringComparison.OrdinalIgnoreCase);
                    break;

                default:
                    throw new BusinessException($"Unsupported question type: {question.QuestionType}");
            }

            // Update the user answer properties
            userAnswer.QuestionId = request.QuestionId;
            userAnswer.OptionId = request.OptionId;
            userAnswer.QuizResultId = request.QuizResultId;
            userAnswer.TextAnswer = request.TextAnswer;
            userAnswer.IsCorrect = isCorrect;
            userAnswer.UpdatedDate = DateTime.UtcNow;

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
        // Get the question with its options
        var question = await _questionReadRepository.GetWhere(q => q.Id == request.QuestionId)
            .Include(q => q.Options)
            .FirstOrDefaultAsync();

        if (question == null)
            throw new NotFoundException($"Question with ID {request.QuestionId} not found.");

        if (!question.IsActive)
            throw new BusinessException($"Question with ID {request.QuestionId} is not active.");

        // Determine if the answer is correct based on question type
        bool isCorrect = false;

        switch (question.QuestionType)
        {
            case QuestionType.SingleChoice:
                // For single choice questions, check if the selected option is marked as correct
                var selectedOption = question.Options.FirstOrDefault(o => o.Id == request.OptionId);
                if (selectedOption == null)
                    throw new BusinessException($"Selected option with ID {request.OptionId} not found for question.");
                
                isCorrect = selectedOption.IsCorrect;
                break;

            case QuestionType.TrueFalse:
                // For true/false questions, check if the selected option matches the correct answer
                var tfSelectedOption = question.Options.FirstOrDefault(o => o.Id == request.OptionId);
                if (tfSelectedOption == null)
                    throw new BusinessException($"Selected option with ID {request.OptionId} not found for question.");
                
                isCorrect = tfSelectedOption.IsCorrect;
                break;

            case QuestionType.ShortAnswer:
                // For short answer questions, compare with the correct option's text
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                if (correctOption == null)
                    throw new BusinessException("No correct option found for short answer question.");

                isCorrect = !string.IsNullOrEmpty(request.TextAnswer) && 
                           !string.IsNullOrEmpty(correctOption.OptionText) &&
                           request.TextAnswer.Trim().Equals(correctOption.OptionText.Trim(), StringComparison.OrdinalIgnoreCase);
                break;

            default:
                throw new BusinessException($"Unsupported question type: {question.QuestionType}");
        }

        // Create the user answer with the calculated isCorrect value
        var userAnswer = _mapper.Map<UserAnswer>(request);
        userAnswer.IsCorrect = isCorrect;

        await _userAnswerWriteRepository.AddAsync(userAnswer);
        await _userAnswerWriteRepository.SaveAsync();
        return _mapper.Map<UserAnswerResponse>(userAnswer);
    }
}