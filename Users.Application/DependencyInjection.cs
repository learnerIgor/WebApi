using Common.Api;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Application.Mapping;

namespace Users.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<IRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IRepository<ApplicationUserRole>, BaseRepository<ApplicationUserRole>>();

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddSingleton<UsersMemoryCache>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
