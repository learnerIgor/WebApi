using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthApplication(this IServiceCollection services)
        {
            services.AddTransient<IRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IRepository<RefreshToken>, BaseRepository<RefreshToken>>();

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
