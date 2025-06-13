using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories.QuizQuestion;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.QuizQuestion;

public class QuizQuestionReadRepository : ReadRepository<Domain.Entities.QuizQuestion>, IQuizQuestionReadRepository
{
    private readonly QuizAppDbContext _context;

    public QuizQuestionReadRepository(QuizAppDbContext context) : base(context)
    {
        _context = context;
    }
    public DbSet<Domain.Entities.QuizQuestion> Table => _context.Set<Domain.Entities.QuizQuestion>();

    public async Task<List<Domain.Entities.QuizQuestion>> GetByQuizIdAsync(Guid quizId)
    {
        return await Table
            .Include(qq => qq.Question)
            .ThenInclude(q => q.Options)
            .Where(qq => qq.QuizId == quizId)
            .OrderBy(qq => qq.OrderIndex)
            .ToListAsync();
    }

    public async Task<Domain.Entities.QuizQuestion?> GetByQuizIdAndQuestionIdAsync(Guid quizId, Guid questionId)
    {
        return await Table
            .Include(qq => qq.Question)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);
    }

    public IQueryable<Domain.Entities.QuizQuestion> GetAllWithDetails()
    {
        return Table
            .Include(x => x.Question)
            .ThenInclude(x => x.Options)
            .Include(x => x.Quiz);
    }
}