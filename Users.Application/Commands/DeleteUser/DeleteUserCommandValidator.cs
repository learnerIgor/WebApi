using FluentValidation;

namespace Users.Application.Commands.DeleteUser
{
    public class DeleteUserCommandValidator: AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator() 
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
        }
    }
}
