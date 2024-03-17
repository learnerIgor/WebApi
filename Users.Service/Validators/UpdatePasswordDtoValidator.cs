using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class UpdatePasswordDtoValidator: AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator() 
        {
            RuleFor(n => n.PasswordHash).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
