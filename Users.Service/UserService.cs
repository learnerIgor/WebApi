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

            if (_userRepository.GetList().Length == 0)
            {
                userRepository.Add(new User { Name = "Tom" });
                userRepository.Add(new User { Name = "Bob" });
                userRepository.Add(new User { Name = "Allice" });
                userRepository.Add(new User { Name = "John" });
                userRepository.Add(new User { Name = "Marty" });
                userRepository.Add(new User { Name = "Lionel" });
                userRepository.Add(new User { Name = "Garry" });
                userRepository.Add(new User { Name = "Tim" });
                userRepository.Add(new User { Name = "Max" });
                userRepository.Add(new User { Name = "Berta" });
            }
        }

        public IReadOnlyCollection<User> GetListUsers(int? offset, string? nameFree, int? limit = 6)
        {
            return _userRepository.GetList(
                offset,
                limit,
                nameFree == null ? null : l => l.Name.Contains(nameFree),
                u => u.Id);
        }

        public User GetIdUser(int id)
        {
            User? user = _userRepository.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                Log.Error($"There isn't user with id {id} in list");
                throw new NotFoundException($"There isn't user with id {id} in list");
            }

            return user;
        }

        public async Task<User> GetIdUserAsync(int id, CancellationToken cancellationToken)
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

        public User Update(int id, UpdateUserDto updtUser)
        {
            if (string.IsNullOrWhiteSpace(updtUser.Name))
            {
                Log.Error("Incorrect user's name");
                throw new BadRequestException($"Invalid username");
            }

            var user = GetIdUser(id);
            _mapper.Map(updtUser, user);

            Log.Information("Updated user " + JsonConvert.SerializeObject(user));

            return _userRepository.Update(user);
        }

        public void Delete(int id)
        {
            var deletUser = GetIdUser(id);
            _userRepository.Delete(deletUser);
            Log.Information("Deleted user " + JsonConvert.SerializeObject(deletUser));
        }

        public int Count(string? nameFree)
        {
            return _userRepository.Count(nameFree == null ? null : c => c.Name.Contains(nameFree));
        }
    }
}
