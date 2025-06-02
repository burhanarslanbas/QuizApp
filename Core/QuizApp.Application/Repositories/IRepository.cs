using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.Entities.Common;

namespace QuizApp.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}