using QuizApp.Application.Repositories.UserAnswer;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.UserAnswer
{
    public class UserAnswerWriteRepository : WriteRepository<Domain.Entities.UserAnswer>, IUserAnswerWriteRepository
    {
        public UserAnswerWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}