using ByteBox.FileStore.Application.BackgroundJobs;
using ByteBox.FileStore.Domain.BackgroundJobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteBox.FileStore.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(config => config.RegisterServicesFromAssembly(applicationAssembly));

        services
            .AddFluentValidationAutoValidation(cfg => cfg.DisableDataAnnotationsValidation = true)
            .AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddBackgroundJobs();

        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<IDeleteTrashFilesJob, DeleteTrashFilesJob>();

        return services;
    }
}
