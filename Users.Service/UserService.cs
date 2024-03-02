﻿using AutoMapper;
using Common.Domain;
using Common.Repositories;
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
                userRepository.Add(new User { Id = 1, Name = "Tom" });
                userRepository.Add(new User { Id = 2, Name = "Bob" });
                userRepository.Add(new User { Id = 3, Name = "Allice" });
                userRepository.Add(new User { Id = 4, Name = "John" });
                userRepository.Add(new User { Id = 5, Name = "Marty" });
                userRepository.Add(new User { Id = 6, Name = "Lionel" });
                userRepository.Add(new User { Id = 7, Name = "Garry" });
                userRepository.Add(new User { Id = 8, Name = "Tim" });
                userRepository.Add(new User { Id = 9, Name = "Max" });
                userRepository.Add(new User { Id = 10, Name = "Berta" });
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

        public User? GetIdUser(int id)
        {
            return _userRepository.SingleOrDefault(x => x.Id == id);
        }

        public User Create(CreateUserDto user)
        {
            if (string.IsNullOrEmpty(user.Name))
                throw new Exception($"Invalid username");

            var userEntity = _mapper.Map<CreateUserDto, User>(user);
            userEntity.Id = _userRepository.GetList().Count() == 0 ? 1 : _userRepository.GetList().Max(i => i.Id) + 1;

            return _userRepository.Add(userEntity);
        }

        public User? Update(int id, UpdateUserDto updtUser)
        {
            if (string.IsNullOrWhiteSpace(updtUser.Name))
                throw new Exception($"Invalid username");

            var user = GetIdUser(id);
            if (user == null)
            {
                return null;
            }

            _mapper.Map(updtUser, user);

            return _userRepository.Update(user);
        }

        public bool Delete(int id)
        {
            var deletUser = GetIdUser(id);
            if (deletUser == null)
                return false;

            return _userRepository.Delete(deletUser);
        }

        public int Count(string? nameFree)
        {
            return _userRepository.Count(nameFree == null ? null : c => c.Name.Contains(nameFree, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
