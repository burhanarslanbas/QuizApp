namespace QuizApp.Application.Repositories.QuizQuestion;

public interface IQuizQuestionWriteRepository : IWriteRepository<Domain.Entities.QuizQuestion>
{
    Task<bool> UpdateOrderAsync(Guid quizId, Guid questionId, int newOrder);
    Task<bool> DeleteByQuizIdAndQuestionIdAsync(Guid quizId, Guid questionId);
}