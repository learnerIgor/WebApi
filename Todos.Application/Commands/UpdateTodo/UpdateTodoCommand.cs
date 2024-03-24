using Common.Domain;
using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<ToDo>
    {
        public int Id { get; set; }
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
        public UpdateTodoCommand(int id, UpdateTodoPayLoad updateTodoPayLoad)
        {
            Id = id;
            Label = updateTodoPayLoad.Label;
            IsDone = updateTodoPayLoad.IsDone;
        }
    }
}
