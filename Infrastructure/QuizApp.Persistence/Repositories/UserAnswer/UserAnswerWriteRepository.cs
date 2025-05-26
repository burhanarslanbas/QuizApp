using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class UserAnswerWriteRepository : WriteRepository<UserAnswer>, IUserAnswerWriteRepository
    {
        public UserAnswerWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}