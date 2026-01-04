namespace Architecture.Presentation.Configurations;

// The class is used to configure controllers.
public static class ConfigureController
{
    // Adds and configures controllers for the application.
    public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers();

        return services;
    }
}
