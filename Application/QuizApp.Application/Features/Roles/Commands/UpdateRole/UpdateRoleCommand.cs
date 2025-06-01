using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
} 