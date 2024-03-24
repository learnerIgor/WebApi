using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Common.Application.Abstractions;
using MediatR;

namespace Todos.Application.Queries.GetListTodos
{
    public class GetListTodosQueryHandler: IRequestHandler<GetListTodosQuery, IReadOnlyCollection<ToDo>>
    {
        private readonly IRepository<ToDo> _todoRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly MemoryCache _memoryCache;
        public GetListTodosQueryHandler(IRepository<ToDo> todoRepository, ICurrentUserService currentUserService, TodosMemoryCache todosMemoryCache) 
        {
            _todoRepository = todoRepository;
            _currentUserService = currentUserService;
            _memoryCache = todosMemoryCache.Cache;
        }

        public async Task<IReadOnlyCollection<ToDo>> Handle(GetListTodosQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (_memoryCache.TryGetValue(cachKey, out IReadOnlyCollection<ToDo>? result))
            {
                return result!;
            }
            var checkAdmin = _currentUserService.UserRole.Contains("Admin");

            result = await _todoRepository.GetListAsync(
                request.Offset,
                request.Limit,
                t => (string.IsNullOrWhiteSpace(request.LabelFreeText) || t.Label.Contains(request.LabelFreeText))
                && (request.OwnerTodo == null || t.UserId == request.OwnerTodo)
                && (_currentUserService.CurrentUserId == t.UserId || checkAdmin),
                t => t.Id,
                cancellationToken: cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(3);

            _memoryCache.Set(cachKey, result, cacheEntryOptions);

            return result;
        }
    }
}
