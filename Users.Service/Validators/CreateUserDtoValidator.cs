using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class CreateUserDtoValidator: AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator() 
        {
            RuleFor(n => n.Login).MinimumLength(5).MaximumLength(50).NotEmpty();
            RuleFor(n => n.Password).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
