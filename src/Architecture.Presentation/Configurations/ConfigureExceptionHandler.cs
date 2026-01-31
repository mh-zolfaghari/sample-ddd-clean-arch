using Architecture.Presentation.Commons.Problem;
using Architecture.Presentation.Middlewares;

namespace Architecture.Presentation.Configurations;

internal static class ConfigureExceptionHandler
{
    internal static IServiceCollection AddConfiguredExceptions(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddScoped<ApplicationProblemDetailsFactory>();

        return services;
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
    }
}
