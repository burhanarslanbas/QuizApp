using AutoMapper;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Infrastructure.Managers;

public class QuestionManager : IQuestionService
{
    private readonly IQuestionReadRepository _questionReadRepository;
    private readonly IQuestionWriteRepository _questionWriteRepository;
    private readonly IQuizReadRepository _quizReadRepository;
    private readonly IMapper _mapper;

    public QuestionManager(
        IQuestionReadRepository questionReadRepository, 
        IQuestionWriteRepository questionWriteRepository, 
        IQuizReadRepository quizReadRepository,
        IMapper mapper)
    {
        _questionReadRepository = questionReadRepository;
        _questionWriteRepository = questionWriteRepository;
        _quizReadRepository = quizReadRepository;
        _mapper = mapper;
    }

    private async Task CheckQuizOwnership(Guid quizId, Guid userId)
    {
        var quiz = await _quizReadRepository.GetByIdAsync(quizId);
        if (quiz == null)
            throw new NotFoundException($"Quiz with ID {quizId} not found.");
        
        if (quiz.CreatorId != userId)
            throw new UnauthorizedException("You are not authorized to modify this quiz's questions.");
    }

    public async Task<QuestionDetailResponse> CreateAsync(CreateQuestionRequest request, Guid userId)
    {
        await CheckQuizOwnership(request.QuizId, userId);
        
        var question = _mapper.Map<Question>(request);
        var result = await _questionWriteRepository.AddAsync(question);
        if (!result)
            throw new BusinessException("Failed to create question");

        return _mapper.Map<QuestionDetailResponse>(question);
    }

    public async Task DeleteAsync(DeleteQuestionRequest request, Guid userId)
    {
        var question = await _questionReadRepository.GetByIdAsync(request.Id);
        if (question == null)
            throw new NotFoundException($"Question with ID {request.Id} not found.");

        await CheckQuizOwnership(question.QuizId, userId);

        var result = await _questionWriteRepository.RemoveById(request.Id);
        if (!result)
            throw new BusinessException("Failed to delete question");
    }

    public async Task<bool> DeleteRange(DeleteRangeQuestionRequest request, Guid userId)
    {
        var questions = _questionReadRepository.GetWhere(x => request.Ids.Contains(x.Id)).ToList();
        if (!questions.Any())
            throw new NotFoundException("No questions found with the provided IDs.");

        // Tüm soruların aynı quize ait olduğunu ve kullanıcının yetkisi olduğunu kontrol et
        var quizId = questions.First().QuizId;
        await CheckQuizOwnership(quizId, userId);

        if (questions.Any(q => q.QuizId != quizId))
            throw new BusinessException("All questions must belong to the same quiz.");

        var result = _questionWriteRepository.RemoveRange(questions);
        if (!result)
            throw new BusinessException("Failed to delete questions.");

        return result;
    }

    public List<QuestionDetailResponse> GetAll(GetQuestionsRequest request)
    {
        var questions = _questionReadRepository.GetAll().ToList();
        return _mapper.Map<List<QuestionDetailResponse>>(questions);
    }

    public async Task<QuestionDetailResponse> GetByIdAsync(GetQuestionByIdRequest request)
    {
        try
        {
            var question = await _questionReadRepository.GetByIdAsync(request.Id);
            return _mapper.Map<QuestionDetailResponse>(question);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Question with ID {request.Id} not found.");
        }
    }

    public async Task<QuestionDetailResponse> Update(UpdateQuestionRequest request, Guid userId)
    {
        var question = await _questionReadRepository.GetByIdAsync(request.Id);
        if (question == null)
            throw new NotFoundException($"Question with ID {request.Id} not found.");

        await CheckQuizOwnership(question.QuizId, userId);

        _mapper.Map(request, question);
        var result = _questionWriteRepository.Update(question);
        if (!result)
            throw new BusinessException($"Failed to update question with ID {request.Id}");

        return _mapper.Map<QuestionDetailResponse>(question);
    }
}