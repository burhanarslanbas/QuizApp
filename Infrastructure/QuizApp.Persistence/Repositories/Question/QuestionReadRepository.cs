using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuestionReadRepository : ReadRepository<Question>, IQuestionReadRepository
    {
        public QuestionReadRepository(QuizAppDbContext context) : base(context) { }
    }
}