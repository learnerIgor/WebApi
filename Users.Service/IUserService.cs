using Users.Service.Dto;

namespace Users.Service
{
    public interface IUserService
    {
        Task<GetUserDto> CreateAsync(CreateUserDto userDto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<GetUserDto> GetUserByIdOrDefaultAsync(int id, CancellationToken cancellationToken);
        Task <IReadOnlyCollection<GetUserDto>> GetListUsersAsync(int? offset, string? nameFree, int? limit = 7, CancellationToken cancellationToken = default);
        Task<GetUserDto> UpdateAsync(int id, UpdateUserDto user, CancellationToken cancellationToken);
        Task<GetUserDto> UpdatePasswordAsync(int id, UpdatePasswordDto user, CancellationToken cancellationToken);
        Task<int> CountAsync(string? nameFree, CancellationToken cancellationToken);
    }
}