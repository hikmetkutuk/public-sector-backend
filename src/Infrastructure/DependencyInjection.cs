using Application.Common.Interfaces;
using Infrastructure.Persistence.Dapper;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Infrastructure.Persistence.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>(provider =>
            new SqlConnectionFactory(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<EntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var interceptor = provider.GetRequiredService<EntityInterceptor>();
            options.AddInterceptors(interceptor);
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}