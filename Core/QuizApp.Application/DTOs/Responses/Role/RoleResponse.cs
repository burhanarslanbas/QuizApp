using System;

namespace QuizApp.Application.DTOs.Responses.Role;

public class RoleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public List<string> Claims { get; set; } = new();
}

public class RoleErrorResponse : RoleResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
} 