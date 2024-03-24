using FluentValidation;

namespace Users.Application.Commands.UpdateUser
{
    public class UpdateUserCommandValidator: AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator() 
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
            RuleFor(n => n.Login).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
