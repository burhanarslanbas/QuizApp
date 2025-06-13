namespace QuizApp.Domain.Constants;

public static class Claims
{
    // Quiz Claims
    public const string CreateQuiz = "CreateQuiz";
    public const string UpdateQuiz = "UpdateQuiz";
    public const string DeleteQuiz = "DeleteQuiz";
    public const string ViewQuiz = "ViewQuiz";
    public const string TakeQuiz = "TakeQuiz";

    // Question Claims
    public const string CreateQuestion = "CreateQuestion";
    public const string UpdateQuestion = "UpdateQuestion";
    public const string DeleteQuestion = "DeleteQuestion";
    public const string ViewQuestion = "ViewQuestion";

    // Category Claims
    public const string CreateCategory = "CreateCategory";
    public const string UpdateCategory = "UpdateCategory";
    public const string DeleteCategory = "DeleteCategory";
    public const string ViewCategory = "ViewCategory";

    // User Claims
    public const string CreateUser = "CreateUser";
    public const string UpdateUser = "UpdateUser";
    public const string DeleteUser = "DeleteUser";
    public const string ViewUser = "ViewUser";
}