using Architecture.Domain.Aggregates.Orders;
using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Infrastructure.Persistence.EF.Configurations.EntityConfigurations;

internal sealed class OrderEntityConfig : BaseEntityConfig<Order>
{
    public override void ConfigureEntity(EntityTypeBuilder<Order> builder)
    {
        builder.Metadata.SetSchema(EntitySchema.BASE);

        ConfigureDomainId<OrderId, Guid>(builder);

        builder
            .Property(x => x.OrderNumber)
            .HasColumnType(SqlTypes.VARCHAR)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderDbId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
