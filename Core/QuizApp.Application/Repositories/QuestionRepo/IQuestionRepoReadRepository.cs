namespace QuizApp.Application.Repositories.QuestionRepo;

public interface IQuestionRepoReadRepository : IReadRepository<Domain.Entities.QuestionRepo>
{
    IQueryable<Domain.Entities.QuestionRepo> GetAllWithDetails(bool tracking = true);
    Task<List<Domain.Entities.QuestionRepo>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = true);
}
