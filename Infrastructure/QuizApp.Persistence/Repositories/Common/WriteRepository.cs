using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities.Common;
using QuizApp.Persistence.Contexts;

namespace QuizApp.Persistence.Repositories.Common;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
    private readonly QuizAppDbContext _context;
    public DbSet<T> Table => _context.Set<T>();

    public WriteRepository(QuizAppDbContext context) => _context = context;

    public async Task<bool> AddAsync(T model)
    {
        EntityEntry<T> entityEntry = await Table.AddAsync(model);
        return entityEntry.State == EntityState.Added;
    }
    public async Task<bool> AddRangeAsync(List<T> datas)
    {
        await Table.AddRangeAsync(datas);
        return true;
    }
    public bool Remove(T model)
    {
        EntityEntry<T> entityEntry = Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }
    public bool RemoveRange(List<T> datas)
    {
        Table.RemoveRange(datas);
        return true;
    }
    public bool Update(T model)
    {
        EntityEntry<T> entityEntry = Table.Update(model);
        return entityEntry.State == EntityState.Modified;
    }
    public bool UpdateRange(List<T> models)
    {
        Table.UpdateRange(models);
        return true;
    }
    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
}