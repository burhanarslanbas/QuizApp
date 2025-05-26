using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuestionRepoReadRepository : ReadRepository<QuestionRepo>, IQuestionRepoReadRepository
    {
        public QuestionRepoReadRepository(QuizAppDbContext context) : base(context) { }
    }
}