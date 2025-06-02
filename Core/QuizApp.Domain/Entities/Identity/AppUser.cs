using Microsoft.AspNetCore.Identity;

namespace QuizApp.Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Quizzes = new HashSet<Quiz>();
        QuizResults = new HashSet<QuizResult>();
    }
    public string FullName { get; set; } = default!; // Kullanıcının tam adı

    // Navigation Properties
    public virtual ICollection<Quiz> Quizzes { get; set; }
    public virtual ICollection<QuizResult> QuizResults { get; set; }
}
