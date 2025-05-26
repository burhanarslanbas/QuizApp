using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuizWriteRepository : WriteRepository<Quiz>, IQuizWriteRepository
    {
        public QuizWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}