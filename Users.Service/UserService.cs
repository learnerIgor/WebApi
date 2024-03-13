using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Users.Service.Dto;

namespace Users.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<User>> GetListUsersAsync(int? offset, string? nameFree, int? limit = 7, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetListAsync(offset, limit, nameFree == null ? null : l => l.Name.Contains(nameFree), u => u.Id, cancellationToken: cancellationToken);
        }

        public async Task<User> GetUserByIdOrDefaultAsync(int id, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (user == null)
            {
                Log.Error($"There isn't user with id {id} in list");
                throw new NotFoundException($"There isn't user with id {id} in list");
            }

            return user;
        }

        public async Task<User> CreateAsync(CreateUserDto user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                Log.Error("Incorrect user's name");
                throw new BadRequestException($"Invalid username");
            }
            var userEntity = _mapper.Map<CreateUserDto, User>(user);
            Log.Information("Added new user " + JsonConvert.SerializeObject(userEntity));

            return await _userRepository.AddAsync(userEntity, cancellationToken);
        }

        public async Task<User> UpdateAsync(int id, UpdateUserDto updtUser, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(updtUser.Name))
            {
                Log.Error("Incorrect user's name");
                throw new BadRequestException($"Invalid username");
            }
            var user = await GetUserByIdOrDefaultAsync(id, cancellationToken);
            _mapper.Map(updtUser, user);
            Log.Information("Updated user " + JsonConvert.SerializeObject(user));

            return await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var deletUser = await GetUserByIdOrDefaultAsync(id, cancellationToken);
            Log.Information("Deleted user " + JsonConvert.SerializeObject(deletUser));

            return await _userRepository.DeleteAsync(deletUser, cancellationToken); 
        }

        public async Task<int> CountAsync(string? nameFree, CancellationToken cancellationToken)
        {
            return await _userRepository.CountAsync(nameFree == null ? null : c => c.Name.Contains(nameFree), cancellationToken);
        }
    }
}
