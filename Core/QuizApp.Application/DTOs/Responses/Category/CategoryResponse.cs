namespace QuizApp.Application.DTOs.Responses.Category
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}