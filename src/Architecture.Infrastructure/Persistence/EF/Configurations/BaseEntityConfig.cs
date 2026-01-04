namespace Architecture.Infrastructure.Persistence.EF.Configurations;

internal sealed record EntityConfigOptions(EntityConfigOptions.EntityRowVersionConfiguration? ConfigureRowVersion)
{
    internal sealed record EntityRowVersionConfiguration
    (
        bool HasIndex = false,
        bool IsClustered = false,
        bool IsConsurrencyToken = false
    );
}

// Base configuration for all entities
internal abstract class BaseEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
{
    protected virtual EntityConfigOptions ConfigOptions
        => new
        (
            ConfigureRowVersion: new()
            {
                HasIndex = true,
                IsClustered = false,
                IsConsurrencyToken = false
            }
        );

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        #region Default Configuration

        ConfigurePrimaryKey(builder);
        ConfigureDeletionProps(builder);
        ConfigureRowVersionProps(builder);

        #endregion

        // Custom configuration
        ConfigureEntity(builder);
    }

    public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    protected virtual void ConfigurePrimaryKey(EntityTypeBuilder<TEntity> builder)
    {
        // Configure DbId as Primary Key
        builder
            .HasKey(e => e.Id);
        builder
            .Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }

    protected virtual void ConfigureDomainId<TPrimitive>(EntityTypeBuilder<TEntity> builder)
        where TPrimitive : notnull
    {
        if (typeof(IEntity<TPrimitive>).IsAssignableFrom(typeof(TEntity)))
        {
            // Configure UniqueId (DomainId) with Primitive type
            builder
                .Property(nameof(IEntity<>.DomainId))
                .IsRequired()
                .ValueGeneratedNever();

            builder
                .HasIndex(nameof(IEntity<>.DomainId))
                .IsUnique();
        }
    }

    protected virtual void ConfigureDomainId<TTypedId, TPrimitive>(EntityTypeBuilder<TEntity> builder)
        where TTypedId : struct, ITypedId<TPrimitive>
    {
        if (typeof(IEntity<TTypedId>).IsAssignableFrom(typeof(TEntity)))
        {
            // Configure UniqueId (DomainId) with TypedId value converter
            builder
                .Property(nameof(IEntity<>.DomainId))
                .IsRequired()
                .HasConversion(new TypedIdValueConverter<TTypedId, TPrimitive>())
                .ValueGeneratedNever();

            builder
                .HasIndex(nameof(IEntity<>.DomainId))
                .IsUnique();
        }
    }

    private void ConfigureDeletionProps(EntityTypeBuilder<TEntity> builder)
    {
        builder.ApplySoftDeleteFilter();
    }

    protected virtual void ConfigureRowVersionProps(EntityTypeBuilder<TEntity> builder)
    {
        if (typeof(IRowVersionProps).IsAssignableFrom(typeof(TEntity)))
        {
            // Config RowVersion column
            builder
                .Property(nameof(IRowVersionProps.RowVersion))
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRowVersion()
                .IsConcurrencyToken(ConfigOptions?.ConfigureRowVersion?.IsConsurrencyToken == true)
                .HasComment("To track changes and control concurrency.");

            if (ConfigOptions?.ConfigureRowVersion?.HasIndex == true)
            {
                builder
                    .HasIndex(nameof(IRowVersionProps.RowVersion))
                    .IsClustered(ConfigOptions?.ConfigureRowVersion?.IsClustered == true);
            }
        }
    }
}
