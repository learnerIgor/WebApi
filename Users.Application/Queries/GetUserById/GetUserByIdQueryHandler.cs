using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;
using Users.Application.Dto;
using MediatR;

namespace Users.Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler:IRequestHandler<GetUserByIdQuery,GetUserDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly MemoryCache _usersMemoryCache;
        public GetUserByIdQueryHandler(IRepository<ApplicationUser> userRepository, UsersMemoryCache usersMemoryCache, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _usersMemoryCache = usersMemoryCache.Cache;
        }

        public async Task<GetUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cachKey = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            if (_usersMemoryCache.TryGetValue(cachKey, out GetUserDto? result))
            {
                return result!;
            }

            ApplicationUser? user = await _userRepository.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"There isn't user with id {request.Id} in DB");
                throw new NotFoundException($"There isn't user with id {request.Id} in DB");
            }

            result = _mapper.Map<GetUserDto>(user);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))
                .SetSize(1);

            _usersMemoryCache.Set(cachKey, result, cacheEntryOptions);

            return result;
        }
    }
}
