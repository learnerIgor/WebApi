using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Application.Utils;
using Newtonsoft.Json;
using Serilog;
using Users.Application.Dto;
using MediatR;

namespace Users.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, GetUserDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<ApplicationUserRole> _appUserRoleRepository;
        private readonly UsersMemoryCache _usersMemoryCache;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IRepository<ApplicationUser> userRepository, IRepository<ApplicationUserRole> appUserRoleRepository, UsersMemoryCache usersMemoryCache, IMapper mapper)
        {
            _userRepository = userRepository;
            _appUserRoleRepository = appUserRoleRepository;
            _usersMemoryCache = usersMemoryCache;
            _mapper = mapper;
        }

        public async Task<GetUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Login))
            {
                Log.Error("Incorrect user's login");
                throw new BadRequestException($"Invalid user login");
            }
            if (await _userRepository.SingleOrDefaultAsync(l => l.Login == request.Login.Trim(), cancellationToken) is not null)
            {
                Log.Error("There is user in DB with such login");
                throw new BadRequestException($"There is user in DB with such login");
            }

            var userRole = (await _appUserRoleRepository.SingleOrDefaultAsync(r => r.Name == "Client", cancellationToken))!;

            var entity = new ApplicationUser()
            {
                Login = request.Login,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Roles = new() { new ApplicationUserApplicationRole { ApplicationUserRoleId = userRole.Id } }
            };
            Log.Information("Added new user " + JsonConvert.SerializeObject(entity));
            _usersMemoryCache.Cache.Clear();

            return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(entity, cancellationToken));
        }
    }
}
