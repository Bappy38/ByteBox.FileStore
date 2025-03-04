using ByteBox.FileStore.Application.BackgroundJobs;
using ByteBox.FileStore.Application.MessageHandlers;
using ByteBox.FileStore.Infrastructure.Messages;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexaWrap.SQS.NET.Extensions;

namespace ByteBox.FileStore.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(config => config.RegisterServicesFromAssembly(applicationAssembly));

        services
            .AddFluentValidationAutoValidation(cfg => cfg.DisableDataAnnotationsValidation = true)
            .AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddMessageHandlers(configuration);

        services.AddBackgroundJobs();

        return services;
    }

    private static IServiceCollection AddMessageHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSqs(configuration, sqsBuilder =>
        {
            sqsBuilder.RegisterHandler<ThumbnailGeneratedMessage, ThumbnailGeneratedMessageHandler>();
            sqsBuilder.RegisterHandler<RefreshFolderMessage, RefreshFolderMessageHandler>();
            sqsBuilder.RegisterHandler<FileUploadedMessage, FileUploadedMessageHandler>();
            sqsBuilder.RegisterHandler<FolderDeletedMessage, FolderDeletedMessageHandler>();
        });
        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<DeleteTrashFilesJob>();
        return services;
    }
}
