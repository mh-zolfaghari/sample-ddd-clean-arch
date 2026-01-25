namespace Architecture.Presentation.Commons.Extensions;

public static class ProblemDetailExtensions
{
    public static IActionResult ToEndpointResponse(this Result result)
    {
        var problemDetails = result.ToApiResponse();
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    public static IActionResult ToEndpointResponse<T>(this Result<T> result)
        where T : notnull
    {
        var problemDetails = result.ToApiResponse();
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    public static IResult ToEndPointResponse(this Result result)
    {
        var problemDetails = result.ToApiResponse();

        return Results.Problem
            (
                detail: problemDetails.Detail,
                statusCode: problemDetails.Status,
                title: problemDetails.Title,
                type: problemDetails.Type,
                extensions: problemDetails.Extensions
            );
    }

    public static IResult ToEndPointResponse<T>(this Result<T> result)
        where T : notnull
    {
        var problemDetails = result.ToApiResponse();

        return Results.Problem
            (
                detail: problemDetails.Detail,
                statusCode: problemDetails.Status,
                title: problemDetails.Title,
                type: problemDetails.Type,
                extensions: problemDetails.Extensions
            );
    }

    private static ProblemDetails ToApiResponse(this Result result)
    {
        if (result.IsSuccess)
            return CreateProblemDetails(ErrorStatus.None);
        return CreateProblemDetailsWithErrorType(result.Error!);
    }

    private static ProblemDetails ToApiResponse<T>(this Result<T> result)
        where T : notnull
    {
        if (result.IsSuccess)
        {
            var response = CreateProblemDetails(ErrorStatus.None);
            response.Extensions["Data"] = result.Data;
            return response;
        }
        return CreateProblemDetailsWithErrorType(result.Error!);
    }

    private static ProblemDetails CreateProblemDetailsWithErrorType(Error error)
    {
        return error.Status switch
        {
            ErrorStatus.None
              => CreateProblemDetails(ErrorStatus.None),
            ErrorStatus.Validation
            or ErrorStatus.Unauthorized
            or ErrorStatus.Forbidden
            or ErrorStatus.NotFound
            or ErrorStatus.Conflict
            or ErrorStatus.Internal
            or ErrorStatus.UnprocessableEntity
              => HandleProblemDetails(error),
            _ => new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred.",
                Type = "https://httpstatuses.com/500"
            },
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

    private static string MapErrorTypeToTypeUri(ErrorStatus errorType)
        => errorType switch
        {
            ErrorStatus.None => "https://httpstatuses.com/200",
            ErrorStatus.Validation => "https://httpstatuses.com/400",
            ErrorStatus.Unauthorized => "https://httpstatuses.com/401",
            ErrorStatus.Forbidden => "https://httpstatuses.com/403",
            ErrorStatus.NotFound => "https://httpstatuses.com/404",
            ErrorStatus.Conflict => "https://httpstatuses.com/409",
            ErrorStatus.Internal => "https://httpstatuses.com/500",
            ErrorStatus.UnprocessableEntity => "https://httpstatuses.com/422",
            _ => "https://httpstatuses.com/500",
        };

    private static ProblemDetails CreateProblemDetails(ErrorStatus errorTypes)
    {
        return new ProblemDetails
        {
            Status = MapErrorTypeToStatusCode(errorTypes),
            Title = MapErrorTypeToTitle(errorTypes),
            Detail = MapErrorTypeToDetail(errorTypes),
            Type = MapErrorTypeToTypeUri(errorTypes)
        };
    }

    private static ProblemDetails HandleProblemDetails(Error error)
    {
        var problemDetails = CreateProblemDetails(error.Status);
        problemDetails.Extensions["Errors"] = new Dictionary<string, string[]>
        {
            { error.Code, [string.Empty] }
        };

        return problemDetails;
    }

    private static void AddTraceIdAndInstanceToProblemDetails(ProblemDetails problemDetails, HttpContext? httpContext = null)
    {
        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId is not null)
            problemDetails.Extensions["TraceId"] = traceId;

        if (httpContext is not null)
            problemDetails.Instance = httpContext.Request.Path;
    }

    public static ProblemDetails ToValidationErrorProblemDetails(ValidationApiException exception, HttpContext? httpContext = null)
    {
        var problemDetails = CreateProblemDetails(ErrorStatus.Validation);
        problemDetails.Extensions["Errors"] = exception.Errors.Errors.ToDictionary(x => x.Key, x => x.Value);

        AddTraceIdAndInstanceToProblemDetails(problemDetails, httpContext);
        return problemDetails;
    }

    public static ProblemDetails ToExceptionProblemDetails(Exception exception, bool showMoreDetailsToResponse, HttpContext? httpContext = null)
    {
        var problemDetails = new ProblemDetails
        {
            Status = MapErrorTypeToStatusCode(ErrorStatus.Internal),
            Title = MapErrorTypeToTitle(ErrorStatus.Internal),
            Type = showMoreDetailsToResponse ? exception.GetType().Name : MapErrorTypeToTypeUri(ErrorStatus.Internal)
        };

        if (showMoreDetailsToResponse)
        {
            problemDetails.Detail = exception.Message;
            problemDetails.Extensions["StackTrace"] = exception.StackTrace;
            if (exception.InnerException is not null)
            {
                problemDetails.Extensions["InnerExceptionMessage"] = exception.InnerException?.Message;
                problemDetails.Extensions["InnerExceptionStackTrace"] = exception.InnerException?.StackTrace;
                problemDetails.Extensions["ExceptionType"] = exception.GetType().FullName;
            }
        }
        else
        {
            problemDetails.Detail = MapErrorTypeToDetail(ErrorStatus.Internal);
        }

        AddTraceIdAndInstanceToProblemDetails(problemDetails, httpContext);
        return problemDetails;
    }
}
