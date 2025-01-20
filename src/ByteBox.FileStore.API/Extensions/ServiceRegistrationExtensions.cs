using ByteBox.FileStore.API.Middlewares;
using ByteBox.FileStore.Domain.BackgroundJobs;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Utilities;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureCorsPolicy(configuration)
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                    .Where(ms => ms.Value != null && ms.Value.Errors.Count > 0)
                    .Select(ms => new ValidationError
                    {
                        PropertyName = ms.Key,
                        Message = ms.Value!.Errors.First().ErrorMessage
                    }).ToList();

                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

        services.AddScoped<GlobalExceptionHandlingMiddleware>();

        return services;
    }

    private static IServiceCollection ConfigureCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        var allowedOrigins = config.GetValue<string>(ConfigKey.AllowedOrigins);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins(allowedOrigins!.Split(','))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseBackgroundJobs();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowSpecificOrigin");

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        return app;
    }

    private static WebApplication UseBackgroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IDeleteTrashFilesJob>("DeleteTrashFilesJob", job => job.ExecuteAsync(), app.Configuration[ConfigKey.DeleteTrashFilesJobSchedule]);

        return app;
    }
}
