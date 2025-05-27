namespace QuizApp.Application.DTOs.Requests.User;

public record UpdateRangeUserRequest(
    List<UpdateUserRequest> Users
);