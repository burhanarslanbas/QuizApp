using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities.Common;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly QuizAppDbContext _context;

        public WriteRepository(QuizAppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public async Task<bool> AddAsync(T model)
        {
            // Add the model to the DbSet and save changes
            await Table.AddAsync(model);
            int result = await _context.SaveChangesAsync();
            // Return true if the save operation was successful
            return result > 0;
        }

        public async Task<bool> AddRangeAsync(List<T> models)
        {
            await Table.AddRangeAsync(models);
            int result = await _context.SaveChangesAsync();
            // Return true if the save operation was successful
            return result > 0;
        }

        public async Task<bool> RemoveById(Guid id)
        {
            T? model = await Table.FirstOrDefaultAsync(data => data.Id.Equals(id));
            if (model == null)
                return false;
            Table.Remove(model);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public bool RemoveRange(List<T> models)
        {
            Table.RemoveRange(models);
            int result = _context.SaveChanges();
            // Return true if the remove operation was successful
            return result > 0;
        }

        public bool Update(T model)
        {
            EntityEntry<T> entry = _context.Entry(model);
            if (entry.State == EntityState.Detached)
                Table.Attach(model);
            entry.State = EntityState.Modified;
            int result = _context.SaveChanges();
            // Return true if the update operation was successful
            return result > 0;
        }
    }
}
