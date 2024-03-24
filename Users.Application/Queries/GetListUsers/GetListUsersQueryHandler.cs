using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Users.Application.Dto;
using MediatR;

namespace Users.Application.Queries.GetListUsers
{
    public class GetListUsersQueryHandler: IRequestHandler<GetListUsersQuery, IReadOnlyCollection<GetUserDto>>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly MemoryCache _usersMemoryCache;
        public GetListUsersQueryHandler(IRepository<ApplicationUser> userRepository, UsersMemoryCache usersMemoryCache, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _usersMemoryCache = usersMemoryCache.Cache;
        }

        public async Task<IReadOnlyCollection<GetUserDto>> Handle(GetListUsersQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (_usersMemoryCache.TryGetValue(cachKey, out IReadOnlyCollection<GetUserDto>? result))
            {
                return result!;
            }

            result = _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _userRepository.GetListAsync(
                request.Offset,
                request.Limit,
                request.NameFree == null ? null : l => l.Login.Contains(request.NameFree),
                u => u.Id,
                cancellationToken: cancellationToken));

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(3);

            _usersMemoryCache.Set(cachKey, result, cacheEntryOptions);

            return result;
        }
    }
}
