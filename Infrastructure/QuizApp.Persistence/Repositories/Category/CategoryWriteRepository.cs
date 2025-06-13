using QuizApp.Application.Repositories.Category;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Category
{
    public class CategoryWriteRepository : WriteRepository<Domain.Entities.Category>, ICategoryWriteRepository
    {
        public CategoryWriteRepository(QuizAppDbContext context) : base(context) { }
    }
}