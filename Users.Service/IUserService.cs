using Common.Domain;
using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        Task<User> CreateAsync(CreateUserDto user, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<User> GetUserByIdOrDefaultAsync(int id, CancellationToken cancellationToken);
        Task <IReadOnlyCollection<User>> GetListUsersAsync(int? offset, string? nameFree, int? limit = 7, CancellationToken cancellationToken = default);
        Task<User> UpdateAsync(int id, UpdateUserDto user, CancellationToken cancellationToken);
        Task<int> CountAsync(string? nameFree, CancellationToken cancellationToken);
    }
}