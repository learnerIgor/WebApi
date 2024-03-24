using MediatR;
using Users.Application.Dto;

namespace Users.Application.Queries.GetListUsers
{
    public class GetListUsersQuery : BaseUsersFilter, IRequest<IReadOnlyCollection<GetUserDto>>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
