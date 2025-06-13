using QuizApp.Application.Repositories.Option;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Option
{
    public class OptionReadRepository : ReadRepository<Domain.Entities.Option>, IOptionReadRepository
    {
        public OptionReadRepository(QuizAppDbContext context) : base(context) { }
    }
}