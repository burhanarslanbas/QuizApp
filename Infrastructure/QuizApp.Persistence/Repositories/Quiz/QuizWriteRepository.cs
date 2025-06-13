using QuizApp.Application.Repositories.Quiz;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Quiz
{
    public class QuizWriteRepository : WriteRepository<Domain.Entities.Quiz>, IQuizWriteRepository
    {
        public QuizWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}