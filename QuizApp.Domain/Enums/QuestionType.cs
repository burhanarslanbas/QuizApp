namespace QuizApp.Domain.Enums
{
    public static class QuestionType
    {
        public static readonly Guid SingleChoice = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid MultipleChoice = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid TrueFalse = Guid.Parse("33333333-3333-3333-3333-333333333333");
        public static readonly Guid ShortAnswer = Guid.Parse("44444444-4444-4444-4444-444444444444");
    }
}
