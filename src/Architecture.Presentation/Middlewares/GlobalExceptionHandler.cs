namespace Architecture.Presentation.Middlewares;

internal sealed class GlobalExceptionHandler
    (
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment
    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync
        (
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
    {
        if (httpContext.Response.HasStarted)
            return false;

        logger.LogError(exception, "Unhandled exception occurred");

        switch (exception)
        {
            case ValidationApiException validationException:
                {
                    await HandleValidationException(httpContext, validationException, cancellationToken);
                    return true;
                }
            default:
                {
                    await HandleUnknownException(httpContext, exception, cancellationToken);
                    return true;
                }
        }
    }

    private async Task HandleValidationException
        (
            HttpContext httpContext,
            ValidationApiException exception,
            CancellationToken cancellationToken
        )
    {
        var problemDetails = ProblemDetailExtensions.ToValidationErrorProblemDetails(exception, httpContext);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private async Task HandleUnknownException
        (
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
    {
        var problemDetails = ProblemDetailExtensions.ToExceptionProblemDetails(exception, environment.IsDevelopment(), httpContext);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}
