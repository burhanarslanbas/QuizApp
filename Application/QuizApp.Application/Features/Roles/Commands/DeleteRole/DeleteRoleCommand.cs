using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Result>
{
    public Guid Id { get; set; }
} 