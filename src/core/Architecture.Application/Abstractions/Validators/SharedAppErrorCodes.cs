namespace Architecture.Application.Abstractions.Validators;

[ErrorCodeContainer]
internal static class SharedAppErrorCodes
{
    internal static Error PaginationPageIndexGreaterThanOrEqual(int minValue)
        => Error.Validation
        (
            code: "Pagination.PageIndexGreaterThanOrEqual",
            args: new Dictionary<string, object?>
            {
                [nameof(minValue)] = minValue
            }
        );

    internal static Error PaginationPageSizeGreaterThanOrEqual(int minValue)
        => Error.Validation
        (
            code: "Pagination.PageSizeGreaterThanOrEqual",
            args: new Dictionary<string, object?>
            {
                [nameof(minValue)] = minValue
            }
        );

    internal static Error PaginationPageSizeLessThanOrEqual(int maxValue)
        => Error.Validation
        (
            code: "Pagination.PageSizeLessThanOrEqual",
            args: new Dictionary<string, object?>
            {
                [nameof(maxValue)] = maxValue
            }
        );
}
