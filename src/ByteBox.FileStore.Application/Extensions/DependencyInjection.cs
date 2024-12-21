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

        return services;
    }
}
