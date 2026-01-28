using Architecture.Presentation.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace Architecture.Presentation.Localization.Problem;

public sealed class ApplicationProblemDetailsFactory(IStringLocalizer<Messages> localizer)
{
    public ApplicationProblemDetails Create(Error error, HttpContext? httpContext = null)
    {
        var message = error.Args?.Any() == true
            ? localizer[error.Code, [.. error.Args.Values!]]
            : localizer[error.Code];

        var problem = new ApplicationProblemDetails
        {
            Title = MapErrorTypeToTitle(error.Status),
            Status = MapErrorTypeToStatusCode(error.Status),
            Detail = message,
            ErrorCode = error.Code,
            Severity = error.Severity
        };

        problem.Type = GenerateErrorCodeUrl(problem.Status, error.Code);

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId is not null)
            problem.TraceId = traceId;

        if (httpContext is not null)
            problem.Instance = httpContext.Request.Path;

        return problem;
    }

    private static int MapErrorTypeToStatusCode(ErrorStatus errorType)
        => errorType switch
        {
            ErrorStatus.None => (int)HttpStatusCode.OK,
            ErrorStatus.Validation => (int)HttpStatusCode.BadRequest,
            ErrorStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ErrorStatus.Forbidden => (int)HttpStatusCode.Forbidden,
            ErrorStatus.NotFound => (int)HttpStatusCode.NotFound,
            ErrorStatus.Conflict => (int)HttpStatusCode.Conflict,
            ErrorStatus.Internal => (int)HttpStatusCode.InternalServerError,
            ErrorStatus.UnprocessableEntity => (int)HttpStatusCode.UnprocessableEntity,
            _ => (int)HttpStatusCode.InternalServerError,
        };

    private static string MapErrorTypeToTitle(ErrorStatus errorType)
        => errorType switch
        {
            ErrorStatus.None => "OK",
            ErrorStatus.Validation => "Validation Error",
            ErrorStatus.Unauthorized => "Unauthorized",
            ErrorStatus.Forbidden => "Forbidden",
            ErrorStatus.NotFound => "Not Found",
            ErrorStatus.Conflict => "Conflict",
            ErrorStatus.Internal => "Internal Server Error",
            ErrorStatus.UnprocessableEntity => "Unprocessable Entity",
            _ => "Internal Server Error",
        };

    private static string GenerateErrorCodeUrl(int? statusCode, string errorCode)
        => $"https://kama.ir/api/docs/errors/{(statusCode is null ? "global" : statusCode.ToString())}?#{errorCode}";
}
