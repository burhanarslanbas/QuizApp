namespace QuizApp.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
