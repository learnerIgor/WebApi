using FluentValidation;

namespace Todos.Application.Queries.GetListTodos
{
    public class GetListTodosQueryValidator : AbstractValidator<GetListTodosQuery>
    {
        public GetListTodosQueryValidator() 
        {
            RuleFor(o => o.Offset).GreaterThan(0).When(o => o.Offset.HasValue);
            RuleFor(l => l.Limit).GreaterThan(0).When(l => l.Limit.HasValue);
            RuleFor(o => o.OwnerTodo).GreaterThan(0).When(o => o.OwnerTodo.HasValue);
            RuleFor(n => n.LabelFreeText).MaximumLength(100);
        }
    }
}
