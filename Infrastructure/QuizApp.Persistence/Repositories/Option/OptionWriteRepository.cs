using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class OptionWriteRepository : WriteRepository<Option>, IOptionWriteRepository
    {
        public OptionWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}