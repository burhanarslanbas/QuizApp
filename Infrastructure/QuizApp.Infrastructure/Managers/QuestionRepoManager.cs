using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Requests.QuestionRepo.Read;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class QuestionRepoManager : IQuestionRepoService
{
    private readonly IQuestionRepoWriteRepository _questionRepoWriteRepository;
    private readonly IQuestionRepoReadRepository _questionRepoReadRepository;
    private readonly IQuestionReadRepository _questionReadRepository;
    private readonly IMapper _mapper;

    public QuestionRepoManager(
        IQuestionRepoWriteRepository questionRepoWriteRepository,
        IQuestionRepoReadRepository questionRepoReadRepository,
        IQuestionReadRepository questionReadRepository,
        IMapper mapper)
    {
        _questionRepoWriteRepository = questionRepoWriteRepository;
        _questionRepoReadRepository = questionRepoReadRepository;
        _questionReadRepository = questionReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateQuestionRepoRequest request)
    {
        var questionRepo = _mapper.Map<QuestionRepo>(request);
        return await _questionRepoWriteRepository.AddAsync(questionRepo);
    }

    public Task<List<QuestionRepoDTO>> CreateRangeAsync(List<CreateQuestionRepoRequest> requests)
    {
        var questionRepos = _mapper.Map<List<QuestionRepo>>(requests);
        return _questionRepoWriteRepository.AddRangeAsync(questionRepos)
            .ContinueWith(task => task.Result ? _mapper.Map<List<QuestionRepoDTO>>(questionRepos) : null);
    }

    public Task<bool> Delete(Guid id)
    {
        return _questionRepoWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var questionRepos = _questionRepoReadRepository.GetWhere(qr => ids.Contains(qr.Id)).ToList();
        if (questionRepos == null || !questionRepos.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_questionRepoWriteRepository.RemoveRange(questionRepos));
    }

    public Task<List<QuestionRepoDTO>> GetAll()
    {
        var questionRepos = _questionRepoReadRepository.GetAll();
        if (questionRepos == null || !questionRepos.Any())
        {
            return Task.FromResult(new List<QuestionRepoDTO>());
        }
        return Task.FromResult(_mapper.Map<List<QuestionRepoDTO>>(questionRepos));
    }

    public Task<QuestionRepoDTO> GetById(Guid id)
    {
        var questionRepo = _questionRepoReadRepository.GetByIdAsync(id).Result;
        if (questionRepo == null)
        {
            return Task.FromResult<QuestionRepoDTO>(null);
        }
        return Task.FromResult(_mapper.Map<QuestionRepoDTO>(questionRepo));
    }

    public Task<QuestionRepoDTO> Update(UpdateQuestionRepoRequest request)
    {
        var questionRepo = _mapper.Map<QuestionRepo>(request);
        if (_questionRepoWriteRepository.Update(questionRepo))
        {
            return Task.FromResult(_mapper.Map<QuestionRepoDTO>(questionRepo));
        }
        throw new Exception("QuestionRepo update failed.");
    }

    public Task<List<QuestionRepoDTO>> GetByCreatorId(Guid creatorId)
    {
        var questionRepos = _questionRepoReadRepository.GetWhere(qr => qr.CreatorId == creatorId).ToList();
        return Task.FromResult(_mapper.Map<List<QuestionRepoDTO>>(questionRepos));
    }

    public Task<List<QuestionRepoDTO>> GetPublicRepos()
    {
        var questionRepos = _questionRepoReadRepository.GetWhere(qr => qr.IsPublic && qr.IsActive).ToList();
        return Task.FromResult(_mapper.Map<List<QuestionRepoDTO>>(questionRepos));
    }

    public async Task<bool> CanAddQuestion(Guid repoId)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(repoId);
        if (questionRepo == null || !questionRepo.IsActive)
            return false;

        var questionCount = await _questionReadRepository.GetWhere(q => q.QuestionRepoId == repoId).CountAsync();
        return questionCount < questionRepo.MaxQuestions;
    }

    public async Task<bool> MakePublic(Guid repoId)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(repoId);
        if (questionRepo == null)
            return false;

        questionRepo.IsPublic = true;
        return _questionRepoWriteRepository.Update(questionRepo);
    }

    public async Task<bool> MakePrivate(Guid repoId)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(repoId);
        if (questionRepo == null)
            return false;

        questionRepo.IsPublic = false;
        return _questionRepoWriteRepository.Update(questionRepo);
    }

    public async Task<bool> ActivateRepo(Guid repoId)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(repoId);
        if (questionRepo == null)
            return false;

        questionRepo.IsActive = true;
        return _questionRepoWriteRepository.Update(questionRepo);
    }

    public async Task<bool> DeactivateRepo(Guid repoId)
    {
        var questionRepo = await _questionRepoReadRepository.GetByIdAsync(repoId);
        if (questionRepo == null)
            return false;

        questionRepo.IsActive = false;
        return _questionRepoWriteRepository.Update(questionRepo);
    }

    public async Task<int> GetQuestionCount(Guid repoId)
    {
        return await _questionReadRepository.GetWhere(q => q.QuestionRepoId == repoId).CountAsync();
    }
} 