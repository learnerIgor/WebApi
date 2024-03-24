using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using MediatR;

namespace Users.Application.Queries.GetCounts
{
    public class GetCountUsersQueryHandler: IRequestHandler<GetCountUsersQuery,int>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly MemoryCache _usersMemoryCache;
        public GetCountUsersQueryHandler(IRepository<ApplicationUser> userRepository, UsersMemoryCache usersMemoryCache) 
        {
            _userRepository = userRepository;
            _usersMemoryCache = usersMemoryCache.Cache;
        }

        public async Task<int> Handle(GetCountUsersQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject($"Count: {request.NameFree}", new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (_usersMemoryCache.TryGetValue(cachKey, out int? result))
            {
                return result!.Value;
            }

            result = await _userRepository.CountAsync(request.NameFree == null ? null : c => c.Login.Contains(request.NameFree), cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(2);

            _usersMemoryCache.Set(cachKey, result, cacheEntryOptions);

            return result.Value;
        }
    }
}
