using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class UpdateUserDtoValidator: AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator() 
        {
            RuleFor(n => n.Name).MinimumLength(3).MaximumLength(20).Must(n => n.All(char.IsLetter)).WithMessage("Incorrect user's name");
        }
    }
}
