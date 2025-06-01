using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Queries.GetAllRoles;

public class GetAllRolesQuery : IRequest<Result<List<RoleDto>>>
{
} 