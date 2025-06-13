using QuizApp.Application.Repositories.Question;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Question
{
    public class QuestionWriteRepository : WriteRepository<Domain.Entities.Question>, IQuestionWriteRepository
    {
        public QuestionWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}