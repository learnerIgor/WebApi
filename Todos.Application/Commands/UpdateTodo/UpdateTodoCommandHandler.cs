using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Commands.UpdateTodo
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, ToDo>
    {

        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateTodoCommandHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ToDo> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
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
            _mapper.Map(request, todoEntity);
            todoEntity.UpdatedDate = DateTime.UtcNow;
            Log.Information("Updated todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.UpdateAsync(todoEntity, cancellationToken);
        }
    }
}
