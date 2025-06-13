namespace QuizApp.Application.Repositories.QuizResult;

public interface IQuizResultReadRepository : IReadRepository<Domain.Entities.QuizResult>
{
    Task<List<Domain.Entities.QuizResult>> GetByUserIdAsync(Guid userId);
    Task<List<Domain.Entities.QuizResult>> GetByQuizIdAsync(Guid quizId);
}