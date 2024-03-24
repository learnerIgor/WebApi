using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Queries.GetTodoById
{
    public class GetTodoByIdQueryHandler: IRequestHandler<GetTodoByIdQuery, ToDo?>
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCache;
        public GetTodoByIdQueryHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, TodosMemoryCache todosMemoryCache) 
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _memoryCache = todosMemoryCache.Cache;
        }

        public async Task<ToDo?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            if (_memoryCache.TryGetValue(cachKey, out ToDo? result))
            {
                return result!;
            }
            ToDo? todo = await _todoRepository.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (todo == null)
            {
                Log.Error($"There isn't todo with id {request.Id} in DB");
                throw new NotFoundException(new { request.Id });
            }
            if (_currentUserService.CurrentUserId != todo.UserId && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {request.Id}");
                throw new ForbiddenException();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(1);

            _memoryCache.Set(cachKey, todo, cacheEntryOptions);

            return todo;
        }
    }
}
