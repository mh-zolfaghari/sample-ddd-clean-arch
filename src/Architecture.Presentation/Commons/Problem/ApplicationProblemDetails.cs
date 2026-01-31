namespace Architecture.Presentation.Commons.Problem;

public sealed record ErrorProblemDetail(string Code, string Message, IDictionary<string, object?>? Args = null);
public sealed record ValidationErrorProblemDetail(string Code, string Message, string? Type, IDictionary<string, object?>? Args = null);

public sealed record ErrorProblems(IReadOnlyDictionary<string, IEnumerable<ValidationErrorProblemDetail>> Errors);

public sealed class ApplicationProblemDetails<TErrorInfo> : ProblemDetails
{
    public TErrorInfo Errors { get; set; } = default!;
    public string Severity { get; set; } = default!;
    public string? TraceId { get; set; }
}
