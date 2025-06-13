using QuizApp.Application.Repositories.UserAnswer;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.UserAnswer
{
    public class UserAnswerReadRepository : ReadRepository<Domain.Entities.UserAnswer>, IUserAnswerReadRepository
    {
        public UserAnswerReadRepository(QuizAppDbContext context) : base(context) { }
    }
}