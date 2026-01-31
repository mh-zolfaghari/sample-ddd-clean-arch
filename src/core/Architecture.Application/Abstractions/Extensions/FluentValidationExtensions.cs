namespace Architecture.Application.Abstractions.Extensions;

internal static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithResultErrorValidation<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error validationError)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        ArgumentNullException.ThrowIfNull(validationError, nameof(validationError));

        if (validationError.Status != ErrorStatus.Validation)
            throw new InvalidOperationException($"This method is only acceptable for {nameof(ErrorStatus)}.{nameof(ErrorStatus.Validation)} error situations.");

        return rule.WithState(_ => validationError);
    }
}
