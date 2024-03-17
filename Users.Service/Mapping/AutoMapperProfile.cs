using AutoMapper;
using Common.Domain;
using Users.Service.Dto;

namespace Users.Service.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<UpdatePasswordDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetUserDto>();
        }
    }
}
