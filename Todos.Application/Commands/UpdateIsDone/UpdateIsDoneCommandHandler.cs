using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Commands.UpdateIsDone
{
    public class UpdateIsDoneCommandHandler: IRequestHandler<UpdateIsDoneCommand, object>
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateIsDoneCommandHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
        }

        public async Task<object> Handle(UpdateIsDoneCommand request, CancellationToken cancellationToken)
        {
            ToDo? todoEntity = await _todoRepository.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (todoEntity == null)
            {
                Log.Error($"There isn't todo with id {request.Id} in DB");
                throw new NotFoundException(new { Id = request.Id });
            }
            if (_currentUserService.CurrentUserId != todoEntity.UserId && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {request.Id}");
                throw new ForbiddenException();
            }
            todoEntity.IsDone = request.IsDone;
            await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
            Log.Information("Todo updated using Patch method " + JsonConvert.SerializeObject(todoEntity));

            return new { Id = todoEntity.Id, IsDone = todoEntity.IsDone };
        }
    }
}
