using Microsoft.EntityFrameworkCore;
using QuizApp.Application.Repositories.QuizQuestion;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories.Common;

namespace QuizApp.Persistence.Repositories.QuizQuestion;

public class QuizQuestionWriteRepository : WriteRepository<Domain.Entities.QuizQuestion>, IQuizQuestionWriteRepository
{
    private readonly QuizAppDbContext _context;

    public QuizQuestionWriteRepository(QuizAppDbContext context) : base(context)
    {
        _context = context;
    }
    public DbSet<Domain.Entities.QuizQuestion> Table => _context.Set<Domain.Entities.QuizQuestion>();

    public async Task<bool> UpdateOrderAsync(Guid quizId, Guid questionId, int newOrder)
    {
        var quizQuestion = await Table
            .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);

        if (quizQuestion == null)
            return false;

        //quizQuestion.OrderIndex = newOrder;
        return true;
    }

    public async Task<bool> DeleteByQuizIdAndQuestionIdAsync(Guid quizId, Guid questionId)
    {
        var quizQuestion = await Table
            .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);

        if (quizQuestion == null)
            return false;

        Table.Remove(quizQuestion);
        return true;
    }
}