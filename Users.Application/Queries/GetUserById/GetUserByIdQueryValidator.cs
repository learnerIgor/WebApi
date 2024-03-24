using FluentValidation;

namespace Users.Application.Queries.GetUserById
{
    public class GetUserByIdQueryValidator: AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator() 
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
        }
    }
}
