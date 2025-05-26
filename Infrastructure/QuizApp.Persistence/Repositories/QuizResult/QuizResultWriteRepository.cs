using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuizResultWriteRepository : WriteRepository<QuizResult>, IQuizResultWriteRepository
    {
        public QuizResultWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}