namespace QuizApp.Application.Repositories.Question;

public interface IQuestionReadRepository : IReadRepository<Domain.Entities.Question>
{
    IQueryable<Domain.Entities.Question> GetAllWithDetails(bool tracking = true);
    Task<List<Domain.Entities.Question>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = true);
}