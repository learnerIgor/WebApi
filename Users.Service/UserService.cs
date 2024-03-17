using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Common.Service.Utils;
using Newtonsoft.Json;
using Serilog;
using Users.Service.Dto;

namespace Users.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<ApplicationUserRole> _appUserRoleRepository;
        private readonly IRepository<ApplicationUserApplicationRole> _appUserAppRoleRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<ApplicationUser> userRepository, IRepository<ApplicationUserRole> applicationUserRoleRepository, IRepository<ApplicationUserApplicationRole> appUserAppRoleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _appUserRoleRepository = applicationUserRoleRepository;
            _appUserAppRoleRepository = appUserAppRoleRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<GetUserDto>> GetListUsersAsync(int? offset, string? nameFree, int? limit = 7, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _userRepository.GetListAsync(offset, limit, nameFree == null ? null : l => l.Login.Contains(nameFree), u => u.Id, cancellationToken: cancellationToken));
        }

        public async Task<GetUserDto> GetUserByIdOrDefaultAsync(int id, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userRepository.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (user == null)
            {
                Log.Error($"There isn't user with id {id} in DB");
                throw new NotFoundException($"There isn't user with id {id} in DB");
            }

            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> CreateAsync(CreateUserDto userDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userDto.Login))
            {
                Log.Error("Incorrect user's login");
                throw new BadRequestException($"Invalid user login");
            }
            if (await _userRepository.SingleOrDefaultAsync(l => l.Login == userDto.Login.Trim(), cancellationToken) is not null)
            {
                Log.Error("There is user in DB with such login");
                throw new BadRequestException($"There is user in DB with such login");
            }

            var userRole = (await _appUserRoleRepository.SingleOrDefaultAsync(r => r.Name == "Client", cancellationToken))!;

            var entity = new ApplicationUser()
            {
                Login = userDto.Login,
                PasswordHash = PasswordHasher.HashPassword(userDto.Password),
                Roles = new() { new ApplicationUserApplicationRole { ApplicationUserRoleId = userRole.Id } }
            };
            Log.Information("Added new user " + JsonConvert.SerializeObject(entity));

            return _mapper.Map<GetUserDto>(await _userRepository.AddAsync(entity, cancellationToken));
        }

        public async Task<GetUserDto> UpdateAsync(int currentUserId, int id, UpdateUserDto updtUserDto, CancellationToken cancellationToken)
        {
            var updtUser = await _userRepository.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
            if (updtUser == null)
            {
                Log.Error($"There isn't user with id {id} in DB");
                throw new NotFoundException($"There isn't user with id {id} in DB");
            }
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: x => x.ApplicationUserId == currentUserId,  cancellationToken: cancellationToken);
            if (currentUserId != id && !userRoles.Any(u => u.ApplicationUserRole.Name == "Admin"))
            {
                Log.Error($"Your account {updtUser.Login} doesn't allow editing");
                throw new ForbiddenException();
            }

            _mapper.Map(updtUserDto, updtUser);
            Log.Information("Updated user " + JsonConvert.SerializeObject(updtUserDto.Login));

            return  _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(updtUser, cancellationToken));
        }

        public async Task<GetUserDto> UpdatePasswordAsync(int currentUserId, int id, UpdatePasswordDto passwordDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
            if(user == null)
            {
                Log.Error($"There isn't user with id {id} in DB");
                throw new NotFoundException($"There isn't user with id {id} in DB");
            }
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: u => u.ApplicationUserId == currentUserId,cancellationToken: cancellationToken);
            if (currentUserId != id && !userRoles.Any(n => n.ApplicationUserRole.Name == "Admin"))
            {
                Log.Error($"Your account {user.Login} doesn't allow editing");
                throw new ForbiddenException();
            }

            passwordDto.PasswordHash = PasswordHasher.HashPassword(passwordDto.PasswordHash);
            _mapper.Map(passwordDto, user);
            Log.Information("Updated user password " + JsonConvert.SerializeObject(user.Login));

            return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(user, cancellationToken));
        }

        public async Task<bool> DeleteAsync(int currentUserId, int id, CancellationToken cancellationToken)
        {
            var deletUser = await _userRepository.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
            if (deletUser == null) 
            {
                Log.Error($"There isn't user with id {id} in DB");
                throw new NotFoundException($"There isn't user with id {id} in DB");
            }
            var userRoles = await _appUserAppRoleRepository.GetListAsync(predicate: x => x.ApplicationUserId == currentUserId, cancellationToken: cancellationToken);
            if (currentUserId != id && !userRoles.Any(u => u.ApplicationUserRole.Name == "Admin"))
            {
                Log.Error($"Your account {deletUser.Login} doesn't allow deliting");
                throw new ForbiddenException();
            }

            Log.Information("Deleted user " + JsonConvert.SerializeObject(new { deletUser.Id, deletUser.Login}));

            return await _userRepository.DeleteAsync(deletUser, cancellationToken); 
        }

        public async Task<int> CountAsync(string? nameFree, CancellationToken cancellationToken)
        {
            return await _userRepository.CountAsync(nameFree == null ? null : c => c.Login.Contains(nameFree), cancellationToken);
        }
    }
}
