using ByteBox.FileStore.API.Middlewares;

namespace ByteBox.FileStore.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<GlobalExceptionHandlingMiddleware>();
        return services;
    }

    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
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
}
