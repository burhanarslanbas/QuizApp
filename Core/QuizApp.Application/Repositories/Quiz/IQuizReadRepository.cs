namespace QuizApp.Application.Repositories.Quiz;

public interface IQuizReadRepository : IReadRepository<Domain.Entities.Quiz>
{
    IQueryable<Domain.Entities.Quiz> GetAllByCreator(Guid creatorId);
    Task<Domain.Entities.Quiz> GetByIdWithQuestionsAsync(Guid id);
    Task<List<Domain.Entities.QuizQuestion>> GetQuizQuestionsAsync(Guid quizId);
}