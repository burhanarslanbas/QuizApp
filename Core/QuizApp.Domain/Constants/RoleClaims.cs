namespace QuizApp.Domain.Constants;

public static class RoleClaims
{
    public static readonly string[] StudentClaims = new[]
    {
        Claims.ViewQuiz,
        Claims.TakeQuiz,
        Claims.ViewQuestion,
        Claims.ViewCategory
    };

    public static readonly string[] TeacherClaims = new[]
    {
        Claims.CreateQuiz,
        Claims.UpdateQuiz,
        Claims.DeleteQuiz,
        Claims.CreateQuestion,
        Claims.UpdateQuestion,
        Claims.DeleteQuestion,
        Claims.CreateCategory,
        Claims.UpdateCategory,
        Claims.DeleteCategory
    }.Concat(StudentClaims).ToArray();

    public static readonly string[] AdminClaims = new[]
    {
        Claims.CreateUser,
        Claims.UpdateUser,
        Claims.DeleteUser,
        Claims.ViewUser
    }.Concat(TeacherClaims).ToArray();
} 