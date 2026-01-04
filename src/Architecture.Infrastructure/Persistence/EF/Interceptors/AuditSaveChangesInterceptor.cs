namespace Architecture.Infrastructure.Persistence.EF.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IOperatorRequester _currentOperator;

    public AuditSaveChangesInterceptor(IOperatorRequester currentOperator)
    {
        _currentOperator = currentOperator;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync
        (
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default
        )
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var utcNow = DateTime.UtcNow;
        var entries = eventData.Context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            HandleAuditableEntry(entry, utcNow);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleAuditableEntry(EntityEntry entry, DateTime utcNow)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                SetCreationAudit(entry, utcNow);
                break;
            case EntityState.Modified:
                SetModificationAudit(entry, utcNow);
                break;
            case EntityState.Deleted:
                SetDeletionAudit(entry, utcNow);
                break;
        }
    }

    private void SetCreationAudit(EntityEntry entry, DateTime utcNow)
    {
        if (entry.Entity is ICreatableProps creator)
        {
            ValidateOperatorRequester();

            creator.SetCreated(_currentOperator.OperatorAccountId, utcNow);
        }
    }

    private void SetModificationAudit(EntityEntry entry, DateTime utcNow)
    {
        if (entry.Entity is IModifiableProps modifier)
        {
            ValidateOperatorRequester();

            modifier.SetModified(_currentOperator.OperatorAccountId, utcNow);
        }
    }

    private void SetDeletionAudit(EntityEntry entry, DateTime utcNow)
    {
        if (entry.Entity is not ISoftDeletableProps remover)
            return;

        ValidateOperatorRequester();

        remover.SetDeleted(_currentOperator.OperatorAccountId, utcNow);

        entry.State = EntityState.Modified;
    }

    private void ValidateOperatorRequester()
    {
        if (_currentOperator is null)
            throw new InvalidOperationException("Current operator requester cannot be null.");

        if (_currentOperator.OperatorAccountId == Guid.Empty)
            throw new InvalidOperationException("OperatorAccountId cannot be an empty GUID.");
    }
}
