using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuizReadRepository : ReadRepository<Quiz>, IQuizReadRepository
    {
        public QuizReadRepository(QuizAppDbContext context) : base(context) { }
    }
}