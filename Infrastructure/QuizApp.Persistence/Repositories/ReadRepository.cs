using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities.Common;
using QuizApp.Persistence.Contexts;
using System.Linq.Expressions;

namespace QuizApp.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly QuizAppDbContext _context;
        public ReadRepository(QuizAppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        public async Task<T> GetByIdAsync(Guid id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query.AsNoTracking();
            return await Table.FirstOrDefaultAsync(data => data.Id.Equals(id));
        }
        //=> await Table.FirstOrDefaultAsync(data => data.Id != null && data.Id.Equals(id));
        //return await Table.FindAsync(id);

    }
}
