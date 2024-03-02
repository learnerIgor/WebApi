using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Users.Service.Mapping;

namespace Users.Service
{
    public static class UserServicesDi
    {
        public static IServiceCollection AddUsersServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<IRepository<User>, BaseRepository<User>>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
