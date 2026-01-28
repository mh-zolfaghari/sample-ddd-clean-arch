using Architecture.Domain.Aggregates.Orders;
using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Infrastructure.Persistence.EF.DbContext;

internal sealed class ApplicationDbContext
    (
        DbContextOptions<ApplicationDbContext> options,
        ILogger<ApplicationDbContext> logger
    ) : Microsoft.EntityFrameworkCore.DbContext(options), IUnitOfWork
{
    private readonly ILogger<ApplicationDbContext> _logger = logger;

    internal DbSet<Order> Orders => Set<Order>();
    internal DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(EntitySchema.BASE);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        HandleEntityDeleteBehavior(modelBuilder);
    }

    public new async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ChangeTracker.HasChanges())
                return Result.Success();

            await base.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            LoggingException(ex);
            return ConstantMessages.SaveChangesFailed;
        }
    }

    private static void HandleEntityDeleteBehavior(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }

    private void LoggingException(Exception ex)
    {
        switch (ex)
        {
            case DbUpdateConcurrencyException:
                _logger?.LogCritical(ex, "DataBase_UpdateConcurrency_Exception");
                break;
            case DbUpdateException:
                _logger?.LogCritical(ex, "DataBase_Update_Exception");
                break;
            case ValidationException:
                _logger?.LogCritical(ex, "DataBase_Validation_Exception");
                break;
            case SqlException:
                _logger?.LogCritical(ex, "DataBase_SQL_Exception");
                break;
            case ObjectDisposedException:
                _logger?.LogCritical(ex, "DataBase_ContextObjectDisposed_Exception");
                break;
            case OperationCanceledException:
                _logger?.LogCritical(ex, "DataBase_OperationCanceled_Exception");
                break;
            default:
                _logger?.LogCritical(ex, "DataBase_Exception");
                break;
        }
    }
}
