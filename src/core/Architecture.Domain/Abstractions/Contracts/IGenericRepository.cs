namespace Architecture.Domain.Abstractions.Contracts;

public interface IReadGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity?> GetAsync(long id, CancellationToken cancellationToken);
}

public interface IReadGenericRepository<TEntity, TTypedId> : IReadGenericRepository<TEntity>
    where TEntity : class, IEntity<TTypedId>
    where TTypedId : struct, ITypedId
{
    Task<TEntity?> GetAsync(TTypedId domainId, CancellationToken cancellationToken);
}

public interface IWriteGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    void Create(TEntity entity);
    void Delete(TEntity entity);
}