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
    private readonly IMapper _mapper;

    public QuestionManager(IQuestionReadRepository questionReadRepository, IQuestionWriteRepository questionWriteRepository, IMapper mapper)
    {
        _questionReadRepository = questionReadRepository;
        _questionWriteRepository = questionWriteRepository;
        _mapper = mapper;
    }

    public async Task<QuestionDetailResponse> CreateAsync(CreateQuestionRequest request)
    {
        var question = _mapper.Map<Question>(request);
        var result = await _questionWriteRepository.AddAsync(question);
        if (!result)
            throw new BusinessException("Failed to create question");

        return _mapper.Map<QuestionDetailResponse>(question);
    }

    public async Task DeleteAsync(DeleteQuestionRequest request)
    {
        var result = await _questionWriteRepository.RemoveById(request.Id);
        if (!result)
            throw new NotFoundException($"Question with ID {request.Id} not found.");
    }

    public bool DeleteRange(DeleteRangeQuestionRequest request)
    {
        var questions = _questionReadRepository.GetWhere(x => request.Ids.Contains(x.Id)).ToList();
        if (!questions.Any())
            throw new NotFoundException("No questions found with the provided IDs.");

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

    public QuestionDetailResponse Update(UpdateQuestionRequest request)
    {
        try
        {
            var question = _questionReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, question);
            var result = _questionWriteRepository.Update(question);
            if (!result)
                throw new BusinessException($"Failed to update question with ID {request.Id}");

            return _mapper.Map<QuestionDetailResponse>(question);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Question with ID {request.Id} not found.");
        }
    }
}