using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class OptionReadRepository : ReadRepository<Option>, IOptionReadRepository
    {
        public OptionReadRepository(QuizAppDbContext context) : base(context) { }
    }
}