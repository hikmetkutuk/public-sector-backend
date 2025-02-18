using Application.Common.Interfaces;
using Infrastructure.Persistence.Dapper;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Infrastructure.Persistence.EntityFramework.Interceptors;
using Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
            new SqlConnectionFactory(configuration.GetConnectionString("DefaultConnection") ??
                                     throw new InvalidOperationException()));

        services.AddScoped<EntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var interceptor = provider.GetRequiredService<EntityInterceptor>();
            options.AddInterceptors(interceptor);
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        var redisConnectionString = configuration.GetSection("Redis")["ConnectionString"];
        if (redisConnectionString != null) services.AddSingleton<IRedisCache>(new RedisCache(redisConnectionString));

        return services;
    }
}