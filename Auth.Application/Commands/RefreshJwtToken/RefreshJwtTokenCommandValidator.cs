using FluentValidation;

namespace Auth.Application.Commands.RefreshJwtToken
{
    public class RefreshJwtTokenCommandValidator : AbstractValidator<RefreshJwtTokenCommand>
    {
        public RefreshJwtTokenCommandValidator() 
        {
            RuleFor(r => r.RefreshToken).NotEmpty();
        }
    }
}
