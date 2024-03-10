using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Service.Mapping;

namespace Users.Service
{
    public static class UserServicesDi
    {
        public static IServiceCollection AddUsersServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<IRepository<User>, SqlServerBaseRepository<User>>();
            services.AddTransient<IUserService, UserService>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            return services;
        }
    }
}
