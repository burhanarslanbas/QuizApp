using QuizApp.Domain.Entities.Common;

namespace QuizApp.Application.Repositories;

public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<bool> AddAsync(T model);
    Task<bool> AddRangeAsync(List<T> models);
    bool Remove(T model);
    bool RemoveRange(List<T> models);
    bool Update(T model);
    bool UpdateRange(List<T> models);
    Task<int> SaveAsync();
}