using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class QuestionWriteRepository : WriteRepository<Question>, IQuestionWriteRepository
    {
        public QuestionWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}