using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories.Question;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Question;

public class QuestionReadRepository : ReadRepository<Domain.Entities.Question>, IQuestionReadRepository
{
    private readonly QuizAppDbContext _context;

    public QuestionReadRepository(QuizAppDbContext context) : base(context)
    {
        _context = context;
    }
    public DbSet<Domain.Entities.Question> Table => _context.Set<Domain.Entities.Question>();

    public IQueryable<Domain.Entities.Question> GetAllWithDetails(bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query.Include(q => q.Options);
    }

    public Task<List<Domain.Entities.Question>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query.Where(question => ids.Contains(question.Id)).ToListAsync();
    }
}