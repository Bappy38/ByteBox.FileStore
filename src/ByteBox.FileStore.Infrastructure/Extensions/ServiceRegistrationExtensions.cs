using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using ByteBox.FileStore.Infrastructure.Data.Interceptors;
using ByteBox.FileStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteBox.FileStore.Infrastructure.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabases(configuration)
            .RegisterRepositories();

        return services;
    }

    private static IServiceCollection ConfigureDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<SoftDeletableEntityInterceptor>();
        services.AddSingleton<AuditableEntityInterceptor>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) => 
            options
            .UseSqlServer(connectionString)
            .AddInterceptors(
                sp.GetRequiredService<SoftDeletableEntityInterceptor>(),
                sp.GetRequiredService<AuditableEntityInterceptor>()
            )
        );

        services.AddHealthChecks().AddSqlServer(connectionString!);

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDriveRepository, DriveRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IFolderPermissionRepository, FolderPermissionRepository>();
        return services;
    }
}
