using FluentValidation;

namespace Users.Application.Commands.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
            RuleFor(n => n.PasswordHash).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
