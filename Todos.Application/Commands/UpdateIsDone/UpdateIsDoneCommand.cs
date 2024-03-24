using MediatR;
using Todos.Application.Dtos;

namespace Todos.Application.Commands.UpdateIsDone
{
    public class UpdateIsDoneCommand : IRequest<object>
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public UpdateIsDoneCommand(int id, UpdateIsDonePayLoad updateIsDonePayLoad)
        {
            Id = id;
            IsDone = updateIsDonePayLoad.IsDone;
        }
    }
}
