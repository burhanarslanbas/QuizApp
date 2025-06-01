using MediatR;
using QuizApp.Application.Common.Models;

namespace QuizApp.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<string>? Claims { get; set; }
} 