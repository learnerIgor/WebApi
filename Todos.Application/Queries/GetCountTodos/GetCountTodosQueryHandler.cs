using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Queries.GetCountTodos
{
    public class GetCountTodosQueryHandler: IRequestHandler<GetCountTodosQuery, int>
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCache;
        public GetCountTodosQueryHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, TodosMemoryCache todosMemoryCache)
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _memoryCache = todosMemoryCache.Cache;
        }

        public async Task<int> Handle(GetCountTodosQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject($"Count: {request.LabelFreeText}", new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (_memoryCache.TryGetValue(cachKey, out int? result))
            {
                return result!.Value;
            }
            var checkAdmin = _currentUserService.UserRole.Contains("Admin");
            result = await _todoRepository.CountAsync(
                t => (string.IsNullOrWhiteSpace(request.LabelFreeText) || t.Label.Contains(request.LabelFreeText))
                && (request.OwnerTodo == null || t.UserId == request.OwnerTodo)
                && (_currentUserService.CurrentUserId == t.UserId || checkAdmin), cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(2);

            _memoryCache.Set(cachKey, result, cacheEntryOptions);

            return result.Value;
        }
    }
}
