namespace Architecture.Domain.Shared;

public static class GlobalErrorCodes
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
