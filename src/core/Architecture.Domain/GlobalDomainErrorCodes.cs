namespace Architecture.Domain;

internal static class GlobalDomainErrorCodes
{
    internal static Error NotFound<TInput>(this TInput input, string code, string propName)
    {
        return Error.NotFound
            (
               code: code,
               severity: ErrorSeverity.Business,
               args: new Dictionary<string, object?>
               {
                   [propName] = input
               });
    }
}
