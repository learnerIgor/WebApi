using FluentValidation;

namespace Users.Application.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(n => n.Login).MinimumLength(5).MaximumLength(50).NotEmpty();
            RuleFor(n => n.Password).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
