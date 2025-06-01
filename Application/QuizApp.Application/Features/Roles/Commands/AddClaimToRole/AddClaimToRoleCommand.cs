using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Commands.AddClaimToRole;

public class AddClaimToRoleCommand : IRequest<Result>
{
    public Guid RoleId { get; set; }
    public string ClaimType { get; set; } = null!;
    public string ClaimValue { get; set; } = null!;
    public string? Description { get; set; }
} 