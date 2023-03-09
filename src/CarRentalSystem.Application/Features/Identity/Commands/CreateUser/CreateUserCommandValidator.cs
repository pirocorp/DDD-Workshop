namespace CarRentalSystem.Application.Features.Identity.Commands.CreateUser;

using FluentValidation;

using static Domain.Models.ModelConstants.Common;
using static Domain.Models.ModelConstants.PhoneNumber;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        this.RuleFor(u => u.Email)
            .MinimumLength(MIN_EMAIL_LENGTH)
            .MaximumLength(MAX_EMAIL_LENGTH)
            .EmailAddress()
            .NotEmpty();

        this.RuleFor(u => u.Password)
            .MaximumLength(MAX_NAME_LENGTH)
            .NotEmpty();

        this.RuleFor(u => u.Name)
            .MinimumLength(MIN_NAME_LENGTH)
            .MaximumLength(MAX_NAME_LENGTH)
            .NotEmpty();

        this.RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .Matches(PHONE_NUMBER_REGULAR_EXPRESSION);
    }
}
