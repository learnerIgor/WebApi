using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator() 
        {
            RuleFor(n => n.Login).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
