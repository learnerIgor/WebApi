using Common.Domain;

namespace Common.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<User> UsersList = new()
        {
                new User { Id = 1, Name="Name1" },
                new User { Id = 2, Name="Name2"},
                new User { Id = 3, Name="Name3" },
                new User { Id = 4, Name="Name4" },
                new User { Id = 5, Name="Name5" },
                new User { Id = 6, Name="Name6" },
                new User { Id = 7, Name="Name7" },
                new User { Id = 8, Name="Name8" },
                new User { Id = 9, Name="Name9" },
                new User { Id = 10, Name="Name10" },
        };
        public IReadOnlyCollection<User> GetListUsers(int? offset, string? nameFree, int? limit)
        {
            var users = UsersList;

            if (!string.IsNullOrWhiteSpace(nameFree))
            {
                users = users.Where(l => l.Name.Contains(nameFree, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            users = users.OrderBy(i => i.Id).ToList();

            if (offset != null)
                users = users.Skip(offset.Value).ToList();

            limit ??= 7;

            users = users.Take(limit.Value).ToList();
            return users;
        }
        public User? GetIdUser(int id)
        {
            return UsersList.Where(i => i.Id == id).SingleOrDefault();
        }
        public User AddUser(User user)
        {
            user.Id = UsersList.Max(i => i.Id) + 1;
            UsersList.Add(user);
            return user;
        }
        public bool DeleteUserById(User user)
        {
            var deleteUser = UsersList.SingleOrDefault(i => i.Id == user.Id);
            if (deleteUser != null)
            {
                UsersList.Remove(deleteUser);
                return true;
            }

            return false;
        }
        public User UpdateUser(User user)
        {
            var updateUser = UsersList.Single(i => i.Id == user.Id);
            updateUser.Name = user.Name;
            return updateUser;
        }
    }
}
