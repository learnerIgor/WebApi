using FluentValidation;

namespace Todos.Application.Queries.GetTodoIsDone
{
    public class GetTodoIsDoneQueryValidator: AbstractValidator<GetTodoIsDoneQuery>
    {
        public GetTodoIsDoneQueryValidator() 
        {
            RuleFor(i => i.Id).GreaterThan(0).NotEmpty();
        }
    }
}
