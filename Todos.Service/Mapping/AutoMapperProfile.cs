using AutoMapper;
using Todos.Domain;
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
