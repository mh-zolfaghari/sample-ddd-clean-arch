namespace Architecture.Shared.Commons.Result;

/// <summary>
/// Extension methods for Result and Result<T> to simplify usage
/// </summary>
public static class ResultExtensions
{
    #region Async Extensions for Result<T>

    /// <summary>
    /// Maps the result of an async operation
    /// </summary>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>
        (
            this Task<Result<TIn>> resultTask,
            Func<TIn, TOut> mapper
        )
    {
        var result = await resultTask;
        return result.Map(mapper);
    }

    /// <summary>
    /// Binds the result of an async operation
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>
        (
            this Task<Result<TIn>> resultTask,
            Func<TIn, Task<Result<TOut>>> binder
        )
    {
        var result = await resultTask;
        return result.IsSuccess && result.Data is not null
            ? await binder(result.Data)
            : Result<TOut>.Failure(result.Error!);
    }

    /// <summary>
    /// Executes an action if the async result is successful
    /// </summary>
    public static async Task<Result<T>> OnSuccessAsync<T>
        (
            this Task<Result<T>> resultTask,
            Func<T, Task> action
        )
    {
        var result = await resultTask;
        if (result.IsSuccess && result.Data is not null)
            await action(result.Data);
        return result;
    }

    /// <summary>
    /// Executes an action if the async result is a failure
    /// </summary>
    public static async Task<Result<T>> OnFailureAsync<T>
        (
            this Task<Result<T>> resultTask,
            Func<Error, Task> action
        )
    {
        var result = await resultTask;
        if (result.IsFailure && result.Error is not null)
            await action(result.Error);
        return result;
    }

    #endregion

    #region Conversion Extensions

    /// <summary>
    /// Converts Result<T> to Result
    /// </summary>
    public static Result ToResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? Result.Success()
            : Result.Failure(result.Error!);
    }

    #endregion

    #region Collection Extensions

    /// <summary>
    /// Combines multiple results into a single result
    /// Returns success only if all results are successful
    /// </summary>
    public static Result Combine(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }

    #endregion

    #region Tap Extensions (Side Effects)

    /// <summary>
    /// Performs a side effect without changing the result
    /// </summary>
    public static Result<T> Tap<T>
        (
            this Result<T> result,
            Action<T> action
        )
    {
        if (result.IsSuccess && result.Data is not null)
            action(result.Data);
        return result;
    }

    /// <summary>
    /// Performs an async side effect without changing the result
    /// </summary>
    public static async Task<Result<T>> TapAsync<T>
        (
            this Result<T> result,
            Func<T, Task> action
        )
    {
        if (result.IsSuccess && result.Data is not null)
            await action(result.Data);
        return result;
    }

    #endregion
}
