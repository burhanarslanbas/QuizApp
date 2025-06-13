using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query.Include(q => q.Options);
    }

    public Task<List<Domain.Entities.Question>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = true)
    {
        public QuestionReadRepository(QuizAppDbContext context) : base(context) { }
    }
}