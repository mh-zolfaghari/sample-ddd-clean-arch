using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Infrastructure.Persistence.EF.Configurations.EntityConfigurations;

internal sealed class OrderItemEntityConfig : BaseEntityConfig<OrderItem>
{
    public override void ConfigureEntity(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Metadata.SetSchema(EntitySchema.BASE);

        ConfigureDomainId<Guid>(builder);

        builder
            .Property(x => x.ProductName)
            .HasColumnType(SqlTypes.NVARCHAR)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Quantity)
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
