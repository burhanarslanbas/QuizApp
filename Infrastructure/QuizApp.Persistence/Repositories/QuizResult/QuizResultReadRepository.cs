using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories.QuizResult;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.QuizResult
{
    public class QuizResultReadRepository : ReadRepository<Domain.Entities.QuizResult>, IQuizResultReadRepository
    {
        private readonly QuizAppDbContext _context;
        public QuizResultReadRepository(QuizAppDbContext context) : base(context)
        {
            _context = context;
        }
        public DbSet<Domain.Entities.QuizResult> Table => _context.Set<Domain.Entities.QuizResult>();
        public Task<List<Domain.Entities.QuizResult>> GetByQuizIdAsync(Guid quizId)
        {
            return Table
                .Where(qr => qr.QuizId == quizId)
                .Include(qr => qr.User) // Include User for additional details if needed
                .ToListAsync();
        }

        public Task<List<Domain.Entities.QuizResult>> GetByUserIdAsync(Guid userId)
        {
            return Table
                .Where(qr => qr.UserId == userId)
                .Include(qr => qr.Quiz) // Include Quiz for additional details if needed
                .ToListAsync();
        }
    }
}