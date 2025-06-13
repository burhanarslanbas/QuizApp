using Microsoft.AspNetCore.Identity;

namespace QuizApp.Domain.Entities.Identity;

public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedDate { get; set; }
    public bool IsActive { get; set; } = true;
}