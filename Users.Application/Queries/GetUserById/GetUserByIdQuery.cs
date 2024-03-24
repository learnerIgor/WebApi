using MediatR;
using Users.Application.Dto;

namespace Users.Application.Queries.GetUserById
{
    public class GetUserByIdQuery: IRequest<GetUserDto>
    {
        public int Id { get; set; }
    }
}
