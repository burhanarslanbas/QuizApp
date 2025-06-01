using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Queries.GetRoleClaims;

public class GetRoleClaimsQuery : IRequest<Result<List<ClaimDto>>>
{
    public Guid RoleId { get; set; }
} 