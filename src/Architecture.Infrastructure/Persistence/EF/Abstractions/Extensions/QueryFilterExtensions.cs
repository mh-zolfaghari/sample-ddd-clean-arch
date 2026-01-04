namespace Architecture.Infrastructure.Persistence.EF.Abstractions.Extensions;

internal static class QueryFilterExtensions
{
    internal static void ApplySoftDeleteFilter<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        if (!typeof(IAuditableProps).IsAssignableFrom(typeof(TEntity)))
            return;

        var parameter = Expression.Parameter(typeof(TEntity), "e");
        var recordStateProperty = Expression.Property(parameter, nameof(IAuditableProps.RecordState));
        var deletedValue = Expression.Constant(RecordState.Deleted);
        var notDeletedExpression = Expression.NotEqual(recordStateProperty, deletedValue);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(notDeletedExpression, parameter);

        builder.HasQueryFilter(lambda);
    }
}
