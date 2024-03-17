using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Service
{
    public static class AuthServicesDi
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
            services.AddTransient<IRepository<ApplicationUserApplicationRole>, SqlServerBaseRepository<ApplicationUserApplicationRole>>();
            services.AddTransient<IRepository<RefreshToken>, SqlServerBaseRepository<RefreshToken>>();

            services.AddTransient<IAuthService, AuthService>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            return services;
        }
    }
}
