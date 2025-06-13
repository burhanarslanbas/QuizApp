using QuizApp.Application.Repositories.QuestionRepo;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.QuestionRepo;

public class QuestionRepoWriteRepository : WriteRepository<Domain.Entities.QuestionRepo>, IQuestionRepoWriteRepository
{
    public QuestionRepoWriteRepository(QuizAppDbContext context) : base(context)
    {
    }
}