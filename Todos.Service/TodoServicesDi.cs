using Microsoft.Extensions.DependencyInjection;
using Common.Repositories;
using Todos.Service.Mapping;
using Todos.Domain;
using Common.Domain;

namespace Todos.Service
{
    public static class TodoServicesDi
    {
        public static IServiceCollection AddTodosServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSingleton<ITodoService, TodoService>();
            services.AddTransient<IRepository<User>, BaseRepository<User>>();
            services.AddTransient<IRepository<ToDo>, BaseRepository<ToDo>>();

            return services;
        }
    }
}
