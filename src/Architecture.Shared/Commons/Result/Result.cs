namespace Architecture.Shared.Commons.Result;

// Represents the outcome of an operation, indicating success or failure, and optionally carrying data or error information.
public record Result
{
    #region Main Props

    // Indicates whether the operation was successful.
    public bool IsSuccess { get; init; }

    [JsonIgnore]
    public bool IsFailure => !IsSuccess; // Convenience property to check for failure.

    // Holds the error information if the operation failed.
    public Error? Error { get; init; }
    #endregion

    #region Protected Ctor
    protected Result(bool isSuccess, Error? error)
    {
        // Validate: Success results should not have errors
        if (isSuccess && error is not null && error != Error.None)
            throw new InvalidOperationException("Success result can not have an error.");

        // Validate: Failure results must have an error
        if (!isSuccess && (error is null || error == Error.None))
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }
    #endregion

    #region Factory Methods

    // Creates a failure result with the specified error.
    public static Result Failure(Error error) => new(false, error);

    // Creates a failure result with the specified error and data.
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);

    // Creates a successful result.
    public static Result Success() => new(true, Error.None);

    // Creates a successful result with the specified data.
    public static Result<T> Success<T>(T data) => Result<T>.Success(data);
    #endregion

    #region Functional Methods
    /// <summary>
    /// Executes an action if the result is successful
    /// </summary>
    public Result OnSuccess(Action action)
    {
        if (IsSuccess)
            action();
        return this;
    }

    /// <summary>
    /// Executes an action if the result is a failure
    /// </summary>
    public Result OnFailure(Action<Error> action)
    {
        if (IsFailure && Error is not null)
            action(Error);
        return this;
    }

    /// <summary>
    /// Pattern matching for Result
    /// </summary>
    public T Match<T>
        (
            Func<T> onSuccess,
            Func<Error, T> onFailure
        )
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
    #endregion
}

// Represents the outcome of an operation that returns data of type T, indicating success or failure, and optionally carrying data or error information.
public record Result<T> : Result
{
    // Holds the data if the operation was successful.
    public T? Data { get; private init; }

    private Result
        (
            bool isSuccess,
            T? data,
            Error? error
        ) : base(isSuccess, error)
    {
        Data = data;
    }

    // Creates a successful result with the specified data.
    public static Result<T> Success(T data) => new(true, data, Error.None);

    // Creates a failure result with the specified error.
    public new static Result<T> Failure(Error error) => new(false, default, error);

    #region Functional Methods
    /// <summary>
    /// Transforms the data if the result is successful
    /// </summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
    {
        return IsSuccess && Data is not null
            ? Result<TOut>.Success(mapper(Data))
            : Result<TOut>.Failure(Error!);
    }

    /// <summary>
    /// Chains another operation that returns a Result
    /// </summary>
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
    {
        return IsSuccess && Data is not null
            ? binder(Data)
            : Result<TOut>.Failure(Error!);
    }

    /// <summary>
    /// Executes an action if the result is successful
    /// </summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess && Data is not null)
            action(Data);
        return this;
    }

    /// <summary>
    /// Executes an action if the result is a failure
    /// </summary>
    public new Result<T> OnFailure(Action<Error> action)
    {
        if (IsFailure && Error is not null)
            action(Error);
        return this;
    }

    /// <summary>
    /// Pattern matching for Result<T>
    /// </summary>
    public TOut Match<TOut>
        (
            Func<T, TOut> onSuccess,
            Func<Error, TOut> onFailure
        )
    {
        return IsSuccess && Data is not null ? onSuccess(Data) : onFailure(Error!);
    }

    /// <summary>
    /// Returns the value if successful, otherwise returns the default value
    /// </summary>
    public T? ValueOrDefault(T? defaultValue = default)
    {
        return IsSuccess ? Data : defaultValue;
    }
    #endregion

    #region Implicit Conversion
    // Implicit conversion from T to Result<T> for success cases
    public static implicit operator Result<T>(T value) => Success(value);

    // Implicit conversion from Error to Result<T> for failure cases
    public static implicit operator Result<T>(Error error) => Failure(error);
    #endregion
}
