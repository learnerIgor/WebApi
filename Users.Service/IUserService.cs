using Common.Domain;
using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        Task<User> CreateAsync(CreateUserDto user, CancellationToken cancellationToken);
        void Delete(int id);
        Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        IReadOnlyCollection<User> GetListUsers(int? offset, string? nameFree, int? limit = 7);
        User Update(int id, UpdateUserDto user);
        int Count(string? nameFree);
    }
}