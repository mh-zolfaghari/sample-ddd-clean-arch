using Architecture.Application.Abstractions.Exceptions;
using Architecture.Presentation.Commons.Problem;

namespace Architecture.Presentation.Middlewares;

internal sealed class GlobalExceptionHandler
    (
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment,
        IServiceProvider serviceProvider
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
        using IServiceScope scope = serviceProvider.CreateScope();
        var problemFactory = scope.ServiceProvider.GetRequiredService<ApplicationProblemDetailsFactory>();
        var problemDetails = problemFactory.Create(exception.Errors);

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
        using IServiceScope scope = serviceProvider.CreateScope();
        var problemFactory = scope.ServiceProvider.GetRequiredService<ApplicationProblemDetailsFactory>();
        var problemDetails = problemFactory.Create(ConstantMessages.UnhandleError(args: GetMoreDetailsWhenIsDevelopmentMode(exception)));

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private Dictionary<string, object?>? GetMoreDetailsWhenIsDevelopmentMode(Exception ex)
    {
        if (environment.IsDevelopment())
        {
            var exceptionArgs = new Dictionary<string, object?>
            {
                ["Type"] = ex.GetType().FullName,
                [nameof(ex.Message)] = ex.Message,
                [nameof(ex.StackTrace)] = ex.StackTrace
            };

            if (ex.InnerException is not null)
                exceptionArgs.Add($"{nameof(ex)}.{nameof(ex.InnerException)}", new Dictionary<string, object?>
                {
                    ["Type"] = ex.InnerException.GetType().FullName,
                    [nameof(ex.InnerException.Message)] = ex.InnerException.Message,
                    [nameof(ex.InnerException.StackTrace)] = ex.InnerException.StackTrace,
                });

            return exceptionArgs;
        }
        return null;
    }
}
