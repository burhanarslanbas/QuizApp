using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuestionRepoWriteRepository : WriteRepository<QuestionRepo>, IQuestionRepoWriteRepository
    {
        public QuestionRepoWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}