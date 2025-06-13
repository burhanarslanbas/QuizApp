using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories.QuestionRepo;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.QuestionRepo;

public class QuestionRepoReadRepository : ReadRepository<Domain.Entities.QuestionRepo>, IQuestionRepoReadRepository
{
    private readonly QuizAppDbContext _context;
    public QuestionRepoReadRepository(QuizAppDbContext context) : base(context)
    {
        _context = context;
    }
    public DbSet<Domain.Entities.QuestionRepo> Table => _context.Set<Domain.Entities.QuestionRepo>();

    public IQueryable<Domain.Entities.QuestionRepo> GetAllWithDetails(bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query.Include(qr => qr.Questions);
    }

    public async Task<List<Domain.Entities.QuestionRepo>> GetByIdsAsync(IEnumerable<Guid> ids, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return await query.Where(repo => ids.Contains(repo.Id)).ToListAsync();
    }
}