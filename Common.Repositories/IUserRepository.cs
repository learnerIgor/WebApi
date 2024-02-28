using Common.Domain;

namespace Common.Repositories
{
    public interface IUserRepository
    {
        User? GetIdUser(int id);
        IReadOnlyCollection<User> GetListUsers(int? offset, string? labelFree, int? limit);
        User AddUser(User toDo);
        User UpdateUser(User toDo);
        bool DeleteUserById(User toDo);
    }
}
