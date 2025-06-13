using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.QuestionRepo;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class QuestionRepoManager : IQuestionRepoService
{
    private readonly IQuestionRepoReadRepository _questionRepoReadRepository;
    private readonly IQuestionRepoWriteRepository _questionRepoWriteRepository;
    private readonly IMapper _mapper;

    public QuestionRepoManager(IQuestionRepoReadRepository questionRepoReadRepository, IQuestionRepoWriteRepository questionRepoWriteRepository, IMapper mapper)
    {
        _questionRepoReadRepository = questionRepoReadRepository;
        _questionRepoWriteRepository = questionRepoWriteRepository;
        _mapper = mapper;
    }

    public async Task<QuestionRepoResponse> CreateAsync(CreateQuestionRepoRequest request)
    {
        var questionRepo = _mapper.Map<QuestionRepo>(request);
        var result = await _questionRepoWriteRepository.AddAsync(questionRepo);


        return _mapper.Map<QuestionRepoResponse>(questionRepo);
    }

    public async Task DeleteAsync(DeleteQuestionRepoRequest request)
    {
        var entity = await _questionRepoReadRepository.GetByIdAsync(request.Id);
        var result = _questionRepoWriteRepository.Remove(entity);
        if (!result)
            throw new NotFoundException($"Question repo with ID {request.Id} not found.");
    }

    public async Task DeleteRangeAsync(DeleteRangeQuestionRepoRequest request)
    {
        var questionRepos = await _questionRepoReadRepository.GetWhere(x => request.DeleteQuestionRepos.Select(r => r.Id).Contains(x.Id)).ToListAsync();
        if (!questionRepos.Any())
            throw new NotFoundException("No question repos found with the provided IDs.");
        _questionRepoWriteRepository.RemoveRange(questionRepos.ToList());
        await _questionRepoWriteRepository.SaveAsync();
    }

    public async Task<IEnumerable<QuestionRepoResponse>> GetAllAsync(GetQuestionReposRequest request)
    {
        var questionRepos = await _questionRepoReadRepository.GetAll().ToListAsync();
        return _mapper.Map<List<QuestionRepoResponse>>(questionRepos);
    }

    public async Task<QuestionRepoResponse> GetByIdAsync(GetQuestionRepoByIdRequest request)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(request.Id);
        if (questionRepo == null)
            throw new NotFoundException($"Question repo with ID {request.Id} not found.");

        return _mapper.Map<QuestionRepoResponse>(questionRepo);
    }

    public async Task<IEnumerable<QuestionRepoResponse>> GetByUserAsync(GetQuestionReposByUserRequest request)
    {
        var questionRepos = await _questionRepoReadRepository.GetAll()
            .Where(x => x.CreatorId == request.UserId)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<QuestionRepoResponse>>(questionRepos);
    }

    public async Task<QuestionRepoResponse> UpdateAsync(UpdateQuestionRepoRequest request)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(request.Id);
        _mapper.Map(request, questionRepo);
        var result = _questionRepoWriteRepository.Update(questionRepo);
        if (!result)
            throw new BusinessException($"Failed to update question repo with ID {request.Id}");

        return _mapper.Map<QuestionRepoResponse>(result);
    }

}