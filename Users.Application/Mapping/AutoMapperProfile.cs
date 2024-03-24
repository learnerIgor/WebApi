using AutoMapper;
using Common.Domain;
using Users.Application.Commands.CreateUser;
using Users.Application.Commands.UpdateUser;
using Users.Application.Commands.UpdatePassword;
using Users.Application.Dto;

namespace Users.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserCommand, ApplicationUser>();
            CreateMap<UpdateUserCommand, ApplicationUser>();
            CreateMap<UpdatePasswordCommand, ApplicationUser>();
            CreateMap<ApplicationUser, GetUserDto>();
        }
    }
}
