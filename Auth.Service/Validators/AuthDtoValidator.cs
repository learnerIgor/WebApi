using Auth.Service.Dto;
using FluentValidation;

namespace Auth.Service.Validators
{
    public class AuthDtoValidator: AbstractValidator<AuthDto>
    {
        public AuthDtoValidator() 
        {
            RuleFor(n => n.Login).MinimumLength(3).MaximumLength(50).NotEmpty();
            RuleFor(n => n.Password).MinimumLength(5).MaximumLength(50).NotEmpty();
        }
    }
}
