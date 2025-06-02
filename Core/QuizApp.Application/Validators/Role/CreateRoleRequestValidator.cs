using FluentValidation;
using QuizApp.Application.DTOs.Requests.Role;

namespace QuizApp.Application.Validators.Role;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(200);

        RuleFor(x => x.Claims)
            .NotNull();
    }
} 