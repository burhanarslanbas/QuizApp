namespace QuizApp.Application.Repositories.QuizQuestion;

public interface IQuizQuestionReadRepository : IReadRepository<Domain.Entities.QuizQuestion>
{
    Task<List<Domain.Entities.QuizQuestion>> GetByQuizIdAsync(Guid quizId);
    Task<Domain.Entities.QuizQuestion?> GetByQuizIdAndQuestionIdAsync(Guid quizId, Guid questionId);
    IQueryable<Domain.Entities.QuizQuestion> GetAllWithDetails();
}