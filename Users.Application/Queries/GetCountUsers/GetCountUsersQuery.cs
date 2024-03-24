using MediatR;
using Users.Application.Dto;

namespace Users.Application.Queries.GetCounts
{
    public class GetCountUsersQuery: BaseUsersFilter, IRequest<int>
    {
    }
}
