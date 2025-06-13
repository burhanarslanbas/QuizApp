using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QuizApp.Domain.Entities.Common;

namespace QuizApp.Persistence.Interceptors;

public class BaseEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var dbContext = eventData.Context;
        if (dbContext == null) return result;

        var entries = dbContext.ChangeTracker.Entries<BaseEntity>();
        Console.WriteLine($"Interceptor: Processing {entries.Count()} entities");

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Interceptor: Set CreatedDate for {entry.Entity.GetType().Name}");
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Interceptor: Set UpdatedDate for {entry.Entity.GetType().Name}");
                    break;
                case EntityState.Deleted:
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    Console.WriteLine($"Interceptor: Set DeletedDate for {entry.Entity.GetType().Name}");
                    break;
            }
        }

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext == null) return new ValueTask<InterceptionResult<int>>(result);

        var entries = dbContext.ChangeTracker.Entries<BaseEntity>();
        Console.WriteLine($"Interceptor Async: Processing {entries.Count()} entities");

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Interceptor Async: Set CreatedDate for {entry.Entity.GetType().Name}");
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Interceptor Async: Set UpdatedDate for {entry.Entity.GetType().Name}");
                    break;
                case EntityState.Deleted:
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    Console.WriteLine($"Interceptor Async: Set DeletedDate for {entry.Entity.GetType().Name}");
                    break;
            }
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}