using QuizApp.Domain.Entities.Common;

namespace QuizApp.Domain.Entities;

public class UserAnswer : BaseEntity
{
    public Guid QuestionId { get; set; }  // Bağlı olduğu soru ID'si
    public Guid? OptionId { get; set; } // Seçilen seçenek ID'si (null olabilir)
    public Guid? QuizResultId { get; set; } // Bağlı olduğu quiz sonucu ID'si
    public string? TextAnswer { get; set; }  // ShortAnswer için
    public bool IsCorrect { get; set; } // Cevabın doğruluğu

    // Navigation Properties
    public virtual QuizResult? QuizResult { get; set; } // Bağlı olduğu quiz sonucu
    public virtual Question Question { get; set; } = default!; // Bağlı olduğu soru
    public virtual Option? Option { get; set; } // Seçilen seçenek
}