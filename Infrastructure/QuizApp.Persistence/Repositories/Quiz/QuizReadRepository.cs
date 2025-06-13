using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories.Quiz;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.Quiz;

public class QuizReadRepository : ReadRepository<Domain.Entities.Quiz>, IQuizReadRepository
{
    private readonly QuizAppDbContext _context;
    public QuizReadRepository(QuizAppDbContext context) : base(context)
    {
        _context = context;
    }
    public DbSet<Domain.Entities.Quiz> Table => _context.Set<Domain.Entities.Quiz>();
    /*public async Task<Domain.Entities.Quiz?> GetByIdAsync(string id, bool tracking = true)
      {
          var query = Table.AsQueryable();
          if (!tracking)
              query = query.AsNoTracking();
          return await query
              .Include(q => q.Category)
              .Include(q => q.QuizQuestions)
                  .ThenInclude(qq => qq.Question)
                      .ThenInclude(q => q.Options)
              .FirstOrDefaultAsync(q => q.Id == Guid.Parse(id));
      }
      *//* public IQueryable<Domain.Entities.Quiz> GetAll(bool tracking = true)
      {
          var query = Table.AsQueryable();
          if (!tracking)
              query = query.AsNoTracking();
          return Table
              .Include(q => q.Category)
              .Include(q => q.QuizQuestions);
      }
      */
    public IQueryable<Domain.Entities.Quiz> GetAllByCreator(Guid creatorId)
    {
        return Table
            .Include(x => x.Creator)
            .Where(x => x.CreatorId == creatorId);
    }

    public async Task<Domain.Entities.Quiz> GetByIdWithQuestionsAsync(Guid id)
    {
        return await Table
            .Include(x => x.QuizQuestions)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Domain.Entities.QuizQuestion>> GetQuizQuestionsAsync(Guid quizId)
    {
        return await Table
            .Include(x => x.QuizQuestions)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.Options)
            .Where(x => x.Id == quizId)
            .SelectMany(x => x.QuizQuestions)
            .ToListAsync();
    }
}