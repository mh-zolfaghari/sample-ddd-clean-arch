namespace Architecture.Presentation.Configurations;

internal static class ConfigureController
{
    internal static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers();

        return services;
    }
}
