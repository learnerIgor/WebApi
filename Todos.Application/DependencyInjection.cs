using Microsoft.Extensions.DependencyInjection;
using Todos.Application.Mapping;
using FluentValidation;
using System.Reflection;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Repositories;
using Common.Application.Abstractions;
using Common.Api;

namespace Todos.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTodosApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddTransient<IRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IRepository<ToDo>, BaseRepository<ToDo>>();

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddSingleton<TodosMemoryCache>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
