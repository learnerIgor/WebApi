using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Queries.GetTodoIsDone
{
    public class GetTodoIsDoneQueryHandler: IRequestHandler<GetTodoIsDoneQuery, object>
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCache;
        public GetTodoIsDoneQueryHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, TodosMemoryCache todosMemoryCache)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _memoryCache = todosMemoryCache.Cache;
        }

        public async Task<object> Handle(GetTodoIsDoneQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            if (_memoryCache.TryGetValue(cachKey, out object? result))
            {
                return result!;
            }
            ToDo? todoDone = await _todoRepository.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (todoDone == null)
            {
                Log.Error($"There isn't todo with id {request.Id} in list");
                throw new NotFoundException($"There isn't todo with id {request.Id} in DB");
            }
            if (_currentUserService.CurrentUserId != todoDone.UserId && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account doesn't allow to get todo with id = {request.Id}");
                throw new ForbiddenException();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(1);

            _memoryCache.Set(cachKey, todoDone, cacheEntryOptions);

            return new { Id = todoDone.Id, IsDone = todoDone.IsDone };
        }
    }
}
