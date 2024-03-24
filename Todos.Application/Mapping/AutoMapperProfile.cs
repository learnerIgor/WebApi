using AutoMapper;
using Common.Domain;
using Todos.Application.Commands.CreateTodo;
using Todos.Application.Commands.UpdateTodo;

namespace Todos.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UpdateTodoCommand, ToDo>();
            CreateMap<CreateTodoCommand, ToDo>();
        }
    }
}
