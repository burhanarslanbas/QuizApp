using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuizResultReadRepository : ReadRepository<QuizResult>, IQuizResultReadRepository
    {
        public QuizResultReadRepository(QuizAppDbContext context) : base(context) { }
    }
}