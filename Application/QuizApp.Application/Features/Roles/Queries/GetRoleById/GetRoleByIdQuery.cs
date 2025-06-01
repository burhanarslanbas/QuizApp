using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQuery : IRequest<Result<RoleDto>>
{
    public Guid Id { get; set; }
} 