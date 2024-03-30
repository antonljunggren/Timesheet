using Application.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<TimeSheetDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("TimeSheetDatabase"));
#if DEBUG
                opt.EnableSensitiveDataLogging();
#endif
            }, ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITimeWorkedRepository, TimeWorkedRepository>();

            return services;
        }
    }
}
