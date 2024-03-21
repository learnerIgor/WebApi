﻿using Microsoft.Extensions.DependencyInjection;
using Common.Repositories;
using Todos.Service.Mapping;
using Common.Domain;
using FluentValidation;
using System.Reflection;
using Common.Service;

namespace Todos.Service
{
    public static class TodoServicesDi
    {
        public static IServiceCollection AddTodosServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, SqlServerBaseRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<ToDo>, SqlServerBaseRepository<ToDo>>();

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            return services;
        }
    }
}
