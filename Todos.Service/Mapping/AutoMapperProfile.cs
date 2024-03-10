using AutoMapper;
using Common.Domain;
using Todos.Service.Dto;

namespace Todos.Service.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UpdateToDoDto, ToDo>();
            CreateMap<CreateToDoDto, ToDo>();
        }
    }
}
