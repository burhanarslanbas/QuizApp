using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.QuizQuestion;
using QuizApp.Application.DTOs.Responses.QuizQuestion;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.QuizQuestion;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;
public class QuizQuestionManager : IQuizQuestionService
{
    private readonly IQuizQuestionReadRepository _quizQuestionReadRepository;
    private readonly IQuizQuestionWriteRepository _quizQuestionWriteRepository;
    private readonly IMapper _mapper;

    public QuizQuestionManager(
        IQuizQuestionReadRepository quizQuestionReadRepository,
        IQuizQuestionWriteRepository quizQuestionWriteRepository,
        IMapper mapper)
    {
        _quizQuestionReadRepository = quizQuestionReadRepository;
        _quizQuestionWriteRepository = quizQuestionWriteRepository;
        _mapper = mapper;
    }

    public async Task<QuizQuestionResponse> CreateAsync(CreateQuizQuestionRequest request)
    {
        var quizQuestion = _mapper.Map<QuizQuestion>(request);
        await _quizQuestionWriteRepository.AddAsync(quizQuestion);
        await _quizQuestionWriteRepository.SaveAsync();
        return _mapper.Map<QuizQuestionResponse>(quizQuestion);
    }

    public async Task<QuizQuestionResponse> UpdateAsync(UpdateQuizQuestionRequest request)
    {
        var quizQuestion = await _quizQuestionReadRepository.GetByIdAsync(request.Id);
        if (quizQuestion == null)
            throw new NotFoundException($"Quiz question with ID {request.Id} not found");

        _mapper.Map(request, quizQuestion);
        _quizQuestionWriteRepository.Update(quizQuestion);
        await _quizQuestionWriteRepository.SaveAsync();
        return _mapper.Map<QuizQuestionResponse>(quizQuestion);
    }

    public async Task DeleteAsync(DeleteQuizQuestionRequest request)
    {
        var quizQuestion = await _quizQuestionReadRepository.GetByIdAsync(request.Id);
        if (quizQuestion == null)
            throw new NotFoundException($"Quiz question with ID {request.Id} not found");

        _quizQuestionWriteRepository.Remove(quizQuestion);
        await _quizQuestionWriteRepository.SaveAsync();
    }

    public async Task<QuizQuestionResponse> GetByIdAsync(GetQuizQuestionByIdRequest request)
    {
        var quizQuestion = await _quizQuestionReadRepository.GetAllWithDetails()
            .FirstOrDefaultAsync(qq => qq.Id == request.Id);

        if (quizQuestion == null)
            throw new NotFoundException($"Quiz question with ID {request.Id} not found");

        return _mapper.Map<QuizQuestionResponse>(quizQuestion);
    }

    public async Task<IEnumerable<QuizQuestionResponse>> GetAllAsync(GetQuizQuestionsRequest request)
    {
        var query = _quizQuestionReadRepository.GetAllWithDetails();

        if (request.QuizId.HasValue)
            query = query.Where(qq => qq.QuizId == request.QuizId);

        if (request.QuestionId.HasValue)
            query = query.Where(qq => qq.QuestionId == request.QuestionId);

        var quizQuestions = await query.ToListAsync();
        return _mapper.Map<IEnumerable<QuizQuestionResponse>>(quizQuestions);
    }
}