using ByteBox.FileStore.API.Middlewares;
using ByteBox.FileStore.Domain.BackgroundJobs;
using ByteBox.FileStore.Domain.Utilities;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
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

    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseBackgroundJobs();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        return app;
    }

    private static WebApplication UseBackgroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IDeleteTrashFilesJob>("DeleteTrashFilesJob", job => job.ExecuteAsync(), app.Configuration["BackgroundJobs:DeleteTrashFilesJob:Schedule"]);

        return app;
    }
}
