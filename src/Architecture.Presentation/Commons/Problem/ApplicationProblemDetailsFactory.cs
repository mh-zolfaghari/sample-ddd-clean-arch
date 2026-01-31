using Architecture.Application.Abstractions.Exceptions;
using Architecture.Presentation.Localization.ErrorCodes;
using Microsoft.Extensions.Localization;

namespace Architecture.Presentation.Commons.Problem;

public sealed class ApplicationProblemDetailsFactory(IStringLocalizer<ErrorCodeMessages> localizer, IHttpContextAccessor httpContextAccessor)
{
    public ApplicationProblemDetails<ErrorProblemDetail> Create(Error error)
    {
        GetTrackableParameters(out string? traceId, out string? instance);

        var problem = new ApplicationProblemDetails<ErrorProblemDetail>
        {
            Title = MapErrorTypeToTitle(error.Status),
            Status = MapErrorTypeToStatusCode(error.Status),
            Detail = MapErrorTypeToDetail(error.Status),
            Errors = new ErrorProblemDetail(Code: error.Code, Message: GetErrorMessage(error), Args: error.Args),
            Severity = error.Severity.ToString(),
            TraceId = traceId,
            Instance = instance,
            Extensions = new Dictionary<string, object?> { ["generatedAt"] = DateTime.UtcNow }
        };

        problem.Type = GenerateErrorCodeUrl(problem.Status, error.Code);

        return problem;
    }

    public ApplicationProblemDetails<ErrorProblems> Create(ValidationError validationError)
    {
        var errorMessages = validationError.ErrorCodes
            .ToDictionary
            (
                x => x.Key,
                x => x.Value.Select(error =>
                {
                    return new ValidationErrorProblemDetail
                        (
                            Code: error.Code,
                            Message: GetErrorMessage(error),
                            Type: GenerateErrorCodeUrl(MapErrorTypeToStatusCode(error.Status), error.Code),
                            Args: error.Args
                        );
                })
            );


        GetTrackableParameters(out string? traceId, out string? instance);

        return new ApplicationProblemDetails<ErrorProblems>
        {
            Title = MapErrorTypeToTitle(ErrorStatus.Validation),
            Status = MapErrorTypeToStatusCode(ErrorStatus.Validation),
            Detail = MapErrorTypeToDetail(ErrorStatus.Validation),
            Errors = new ErrorProblems(errorMessages),
            Severity = ErrorSeverity.Business.ToString(),
            Instance = instance,
            TraceId = traceId,
            Extensions = new Dictionary<string, object?> { ["generatedAt"] = DateTime.UtcNow }
        };
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

    private static string MapErrorTypeToDetail(ErrorStatus errorType)
        => errorType switch
        {
            ErrorStatus.None => "The request has succeeded.",
            ErrorStatus.Validation => "One or more validation errors occurred.",
            ErrorStatus.Unauthorized => "Authentication is required and has failed or has not yet been provided.",
            ErrorStatus.Forbidden => "You do not have permission to access this resource.",
            ErrorStatus.NotFound => "The requested resource could not be found.",
            ErrorStatus.Conflict => "A conflict occurred with the current state of the resource.",
            ErrorStatus.Internal => "An internal server error occurred.",
            ErrorStatus.UnprocessableEntity => "The request was well-formed but was unable to be followed due to semantic errors.",
            _ => "An internal server error occurred.",
        };

    private static string GenerateErrorCodeUrl(int? statusCode, string errorCode)
        => $"https://kama.ir/api/docs/errors/{(statusCode is null ? "global" : statusCode.ToString())}?#{errorCode}";

    private void GetTrackableParameters(out string? traceId, out string? instance)
    {
        var httpContext = httpContextAccessor.HttpContext;
        instance = httpContext is not null ? httpContext.Request.Path : null;

        var tId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        traceId = !string.IsNullOrWhiteSpace(tId) ? tId : null;
    }

    private string GetErrorMessage(Error error)
    {
        var localized = localizer[error.Code].Value;

        if (error.Args is null || error.Args.Count == 0)
            return localized;

        foreach (var arg in error.Args)
        {
            localized = localized.Replace($"{{{arg.Key}}}", arg.Value?.ToString());
        }

        return localized;
    }
}
