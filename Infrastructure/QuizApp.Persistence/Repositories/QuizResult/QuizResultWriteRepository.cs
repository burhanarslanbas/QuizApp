using QuizApp.Application.Repositories.QuizResult;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.QuizResult
{
    public class QuizResultWriteRepository : WriteRepository<Domain.Entities.QuizResult>, IQuizResultWriteRepository
    {
        public QuizResultWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}