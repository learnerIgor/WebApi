using FluentValidation;
using Users.Application.Queries.GetCounts;

namespace Users.Application.Queries.GetCountUsers
{
    public class GetCountUsersQueryValidator: AbstractValidator<GetCountUsersQuery>
    {
        public GetCountUsersQueryValidator() 
        {
            RuleFor(n => n.NameFree).MaximumLength(100);
        }
    }
}
