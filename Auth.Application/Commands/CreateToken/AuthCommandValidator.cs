using FluentValidation;

namespace Auth.Application.Commands.CreateToken
{
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(n => n.Login).MinimumLength(3).MaximumLength(50).NotEmpty();
            RuleFor(n => n.Password).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
