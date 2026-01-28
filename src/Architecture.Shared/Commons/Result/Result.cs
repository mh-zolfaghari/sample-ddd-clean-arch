namespace Architecture.Shared.Commons.Result;

public record Result
{
    #region Main Props

    public bool IsSuccess { get; init; }

    [JsonIgnore]
    public bool IsFailure => !IsSuccess; // Convenience property to check for failure.

    public Error? Error { get; init; }
    #endregion

    #region Protected Ctor
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null && error != Error.None)
            throw new InvalidOperationException("Success result can not have an error.");

        if (!isSuccess && (error is null || error == Error.None))
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }
    #endregion

    #region Factory Methods

    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
    public static Result Success() => new(true, Error.None);
    public static Result<T> Success<T>(T data) => Result<T>.Success(data);
    #endregion

    #region Functional Methods
    public Result OnSuccess(Action action)
    {
        if (IsSuccess)
            action();
        return this;
    }

    public Result OnFailure(Action<Error> action)
    {
        if (IsFailure && Error is not null)
            action(Error);
        return this;
    }

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

public record Result<T> : Result
{
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

    public static Result<T> Success(T data) => new(true, data, Error.None);
    public new static Result<T> Failure(Error error) => new(false, default, error);

    #region Functional Methods
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
    {
        return IsSuccess && Data is not null
            ? Result<TOut>.Success(mapper(Data))
            : Result<TOut>.Failure(Error!);
    }

    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
    {
        return IsSuccess && Data is not null
            ? binder(Data)
            : Result<TOut>.Failure(Error!);
    }

    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess && Data is not null)
            action(Data);
        return this;
    }

    public new Result<T> OnFailure(Action<Error> action)
    {
        if (IsFailure && Error is not null)
            action(Error);
        return this;
    }

    public TOut Match<TOut>
        (
            Func<T, TOut> onSuccess,
            Func<Error, TOut> onFailure
        )
    {
        return IsSuccess && Data is not null ? onSuccess(Data) : onFailure(Error!);
    }

    public T? ValueOrDefault(T? defaultValue = default)
    {
        return IsSuccess ? Data : defaultValue;
    }
    #endregion

    #region Implicit Conversion
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
    #endregion
}
