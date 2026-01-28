namespace Architecture.Presentation;

public static class AppServiceConfigurations
{
    public static void ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguredHttp();
        services.AddConfiguredControllers();
        services.AddConfiguredExceptions();
        services.AddConfiguredLocalization();
    }
}
