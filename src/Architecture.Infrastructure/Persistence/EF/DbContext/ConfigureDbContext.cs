using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Architecture.Infrastructure.Persistence.EF.DbContext;

internal static class ConfigureDbContext
{
    internal static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOperatorRequester, FakeCurrentOperator>();
        services.TryAddSingleton<PublishDomainEventsSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("MigrationHistory");
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            }).LogTo(Console.WriteLine, LogLevel.Information);

            options.AddInterceptors(new AuditSaveChangesInterceptor(serviceProvider.GetRequiredService<IOperatorRequester>()));
            options.AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsSaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
}
