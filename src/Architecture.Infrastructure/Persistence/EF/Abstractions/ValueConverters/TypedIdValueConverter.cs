namespace Architecture.Infrastructure.Persistence.EF.Abstractions.ValueConverters;

// The TypedIdValueConverter class is a generic value converter for typed IDs.
public class TypedIdValueConverter<TTypedId, TPrimitive> : ValueConverter<TTypedId, TPrimitive>
    where TTypedId : struct, ITypedId<TPrimitive>
{
    public TypedIdValueConverter()
        : base
          (
              id => id.Value,
              value => (TTypedId)Activator.CreateInstance(typeof(TTypedId), value)!
          )
    { }
}
