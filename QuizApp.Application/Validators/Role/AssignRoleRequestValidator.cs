using FluentValidation;
using QuizApp.Application.DTOs.Requests.Role;
using QuizApp.Domain.Constants;

namespace QuizApp.Application.Validators.Role;

public class AssignRoleRequestValidator : AbstractValidator<AssignRoleRequest>
{
    public AssignRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required")
            .Must(role => typeof(RoleConstants.Roles)
                .GetFields()
                .Any(f => f.GetValue(null)?.ToString() == role))
            .WithMessage("Invalid role name");
    }
} 