using Amazon;
using Amazon.S3;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Domain.Utilities;
using ByteBox.FileStore.Infrastructure.Data;
using ByteBox.FileStore.Infrastructure.Data.Interceptors;
using ByteBox.FileStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ByteBox.FileStore.Infrastructure.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddRepositories()
            .AddStorage(configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SoftDeletableEntityInterceptor>();
        services.AddScoped<AuditableEntityInterceptor>();

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

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDriveRepository, DriveRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IFolderPermissionRepository, FolderPermissionRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFilePermissionRepository, FilePermissionRepository>();
        return services;
    }

    private static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region)
            };

            return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, config);
        });

        return services;
    }
}
