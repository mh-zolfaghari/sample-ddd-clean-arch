namespace Architecture.Domain.Abstractions.Contracts;

public interface IUnitOfWork
{
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}
