using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities.Common;
using QuizApp.Persistence.Contexts;
using System.Linq.Expressions;

namespace QuizApp.Persistence.Repositories.Common;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly QuizAppDbContext _context;

    public ReadRepository(QuizAppDbContext context) => _context = context;

    public DbSet<T> Table => _context.Set<T>();

    public virtual IQueryable<T> GetAll(bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query;
    }

    public async Task<T> GetByIdAsync(Guid id, bool tracking = true)
    //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
    //=> await Table.FindAsync(Guid.Parse(id));
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(data => data.Id == id);
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(method);
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.Where(method);
        if (!tracking)
            query = query.AsNoTracking();
        return query;
    }
}