using FluentValidation;

namespace Todos.Application.Queries.GetTodoById
{
    public class GetTodoByIdQueryValidator: AbstractValidator<GetTodoByIdQuery>
    {
        public GetTodoByIdQueryValidator() 
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
        }
    }
}
