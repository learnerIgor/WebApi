using Common.Domain;

namespace Users.Service
{
    public interface IUserService
    {
        User Create(User user);
        bool Delete(int id);
        User? GetIdUser(int id);
        IReadOnlyCollection<User> GetListUsers(int? offset, string? labelFree, int? limit = 7);
        User? Update(User user);
    }
}