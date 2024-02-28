using Common.Domain;
using Common.Repositories;

namespace Users.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IReadOnlyCollection<User> GetListUsers(int? offset, string? labelFree, int? limit = 7)
        {
            return _userRepository.GetListUsers(offset, labelFree, limit);
        }

        public User? GetIdUser(int id)
        {
            return _userRepository.GetIdUser(id);
        }

        public User Create(User user)
        {
            return _userRepository.AddUser(user);
        }
        public User? Update(User user)
        {
            var updtUser = _userRepository.GetIdUser(user.Id);
            if (updtUser == null)
            {
                return null;
            }
            return _userRepository.UpdateUser(user);
        }
        public bool Delete(int id)
        {
            var deletUser = this.GetIdUser(id);
            if (deletUser == null)
                return false;

            return _userRepository.DeleteUserById(deletUser);
        }
    }
}
