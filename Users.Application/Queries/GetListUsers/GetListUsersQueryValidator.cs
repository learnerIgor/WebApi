using FluentValidation;

namespace Users.Application.Queries.GetListUsers
{
    public class GetListUsersQueryValidator: AbstractValidator<GetListUsersQuery>
    {
        public GetListUsersQueryValidator() 
        {
            RuleFor(o => o.Offset).GreaterThan(0).When(o => o.Offset.HasValue);
            RuleFor(l => l.Limit).GreaterThan(0).When(l => l.Limit.HasValue);
            RuleFor(n => n.NameFree).MaximumLength(100);
        }
    }
}
