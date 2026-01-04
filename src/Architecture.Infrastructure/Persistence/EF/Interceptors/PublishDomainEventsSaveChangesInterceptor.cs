using Architecture.Shared.Commons.CQRS.Contracts;

namespace Architecture.Infrastructure.Persistence.EF.Interceptors;

public sealed class PublishDomainEventsSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PublishDomainEventsSaveChangesInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async ValueTask<int> SavedChangesAsync
        (
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default
        )
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(Microsoft.EntityFrameworkCore.DbContext context, CancellationToken cancellationToken)
    {
        var domainEvents = context
            .ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToArray();

        if (domainEvents?.Length > 0)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<IMediator>();
            await publisher.PublishAsync(cancellationToken, domainEvents);
        }
    }
}
