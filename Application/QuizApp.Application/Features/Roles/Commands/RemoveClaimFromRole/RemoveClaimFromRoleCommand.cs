using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Commands.RemoveClaimFromRole;

public class RemoveClaimFromRoleCommand : IRequest<Result>
{
    public Guid RoleId { get; set; }
    public int ClaimId { get; set; }
} 