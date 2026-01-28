namespace Architecture.Presentation.Configurations;

internal static class ConfigureHttp
{
    internal static IServiceCollection AddConfiguredHttp(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddProblemDetails();

        return services;
    }
}