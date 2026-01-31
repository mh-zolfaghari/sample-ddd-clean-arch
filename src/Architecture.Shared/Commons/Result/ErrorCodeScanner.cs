namespace Architecture.Shared.Commons.Result;

public static class ErrorCodeScanner
{
    public static IReadOnlyDictionary<string, Error> Scan(params Assembly[] assemblies)
    {
        var result = new Dictionary<string, Error>();

        foreach (var assembly in assemblies)
        {
            var errorContainers = assembly.GetTypes()
                .Where(t => t.IsClass && t.IsAbstract && t.IsSealed && t.GetCustomAttribute<ErrorCodeContainerAttribute>() != null);

            foreach (var type in errorContainers)
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

                // Properties
                foreach (var prop in type.GetProperties(flags).Where(p => p.PropertyType == typeof(Error)))
                {
                    Add(result, prop.GetValue(null) as Error, prop);
                }

                // Fields
                foreach (var field in type.GetFields(flags).Where(f => f.FieldType == typeof(Error)))
                {
                    Add(result, field.GetValue(null) as Error, field);
                }

                // Methods
                foreach (var method in type.GetMethods(flags).Where(m => m.ReturnType == typeof(Error) && m.GetParameters().Length == 0))
                {
                    Add(result, method.Invoke(null, null) as Error, method);
                }
            }
        }

        return result;
    }

    private static void Add(IDictionary<string, Error> dict, Error? error, MemberInfo source)
    {
        if (error is null)
            return;

        if (string.IsNullOrWhiteSpace(error.Code))
            throw new InvalidOperationException($"Error code is empty in {source.DeclaringType?.FullName}.{source.Name}");

        if (!dict.TryAdd(error.Code, error))
            throw new InvalidOperationException($"Duplicate error code '{error.Code}' detected.");
    }
}
