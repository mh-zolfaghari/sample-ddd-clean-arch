namespace Architecture.Presentation.Localization.Problem;

public sealed class ApplicationProblemDetails : ProblemDetails
{
    public string ErrorCode { get; set; } = default!;
    public ErrorSeverity Severity { get; set; }
    public string? TraceId { get; set; }
}
