using Common.Domain;
using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        User Create(CreateUserDto user);
        void Delete(int id);
        User GetIdUser(int id);
        IReadOnlyCollection<User> GetListUsers(int? offset, string? nameFree, int? limit = 7);
        User Update(int id, UpdateUserDto user);
        int Count(string? nameFree);
    }
}