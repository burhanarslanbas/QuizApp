using QuizApp.Application.Repositories.Category;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Category
{
    public class CategoryReadRepository : ReadRepository<Domain.Entities.Category>, ICategoryReadRepository
    {
        public CategoryReadRepository(QuizAppDbContext context) : base(context) { }
    }
}