namespace Architecture.Infrastructure;

// The AppServiceConfigurations class contains extension methods for configuring infrastructure services.
public static class AppServiceConfigurations
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF DbContext registration
        ConfigureDbContext.RegisterDbContext(services, configuration);

        // Domain Repository registration
        RegisterRepositoris(services);
    }

    private  static void RegisterRepositoris(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}
