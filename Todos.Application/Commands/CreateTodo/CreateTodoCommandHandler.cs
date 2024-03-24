using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Newtonsoft.Json;
using Serilog;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Commands.CreateTodo
{
    public class CreateTodoCommandHandler: IRequestHandler<CreateTodoCommand, ToDo>
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateTodoCommandHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ToDo> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todoEntity = _mapper.Map<CreateTodoCommand, ToDo>(request);
            todoEntity.CreatedDate = DateTime.UtcNow;
            todoEntity.UserId = _currentUserService.CurrentUserId;
            Log.Information("Added new todo " + JsonConvert.SerializeObject(todoEntity));

            return await _todoRepository.AddAsync(todoEntity, cancellationToken);
        }
    }
}
