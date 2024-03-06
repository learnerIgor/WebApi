using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class CreateUserDtoValidator: AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator() 
        {
            RuleFor(n => n.Name).MinimumLength(3).MaximumLength(20).NotNull().Must(n => n.All(char.IsLetter)).WithMessage("Incorret user's name");
        }
    }
}
