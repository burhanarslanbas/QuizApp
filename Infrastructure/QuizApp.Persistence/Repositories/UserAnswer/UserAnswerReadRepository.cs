using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class UserAnswerReadRepository : ReadRepository<UserAnswer>, IUserAnswerReadRepository
    {
        public UserAnswerReadRepository(QuizAppDbContext context) : base(context) { }
    }
}