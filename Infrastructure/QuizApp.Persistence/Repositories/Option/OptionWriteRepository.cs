using QuizApp.Application.Repositories.Option;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Option
{
    public class OptionWriteRepository : WriteRepository<Domain.Entities.Option>, IOptionWriteRepository
    {
        public OptionWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}