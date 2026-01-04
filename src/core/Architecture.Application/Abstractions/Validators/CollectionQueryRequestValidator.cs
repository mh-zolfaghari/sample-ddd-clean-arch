namespace Architecture.Application.Abstractions.Validators;

public abstract class CollectionQueryRequestValidator<TQuery, TResponse> : AbstractValidator<TQuery>
    where TQuery : CollectionQueryRequest<TResponse>
    where TResponse : notnull
{
    protected virtual int MaxPageSize => 50;
    protected virtual bool CanGetAll => true;

    protected CollectionQueryRequestValidator()
    {
        When(x => !(x.PageIndex == 0 && x.PageSize == 0 && CanGetAll), () =>
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("The PageIndex value can't be less than 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("The PageSize value can't be less than 1.")
                .LessThanOrEqualTo(MaxPageSize).WithMessage($"The PageSize value cannot be greater than {MaxPageSize}.");
        });

        When(x => x.SortBy?.Length > 0, () =>
        {
            RuleFor(x => x.SortBy)
                .Must((request, item, context) => request.SortableItems().Length == 0)
                .WithErrorCode("")
                .WithMessage("");

            RuleFor(x => x.SortBy)
                .Must(x => x!.Any(string.IsNullOrWhiteSpace))
                .WithErrorCode("FoundedNullOrEmptySortItem")
                .WithMessage("اینجا باید متن خطای پیدا شدن مقدار خالی یا null رو بنویسی");
        });

        Validations();
    }

    private static bool IsValidSort(TQuery request)
    {
        int inputLength = request.SortBy?.Length ?? 0;

        if (inputLength == 0)
            return true;

        if (request.SortBy!.Any(string.IsNullOrEmpty))
            return false;

        if (request.SortBy!.Distinct().Count() != inputLength)
            return false;

        foreach (var item in request.SortBy!)
        {
            if (!request.SortableItems().Any(x => x.Equals(item, StringComparison.OrdinalIgnoreCase)))
                return false;
        }

        return true;
    }

    private static string SortParameterValidation(string[] validParams)
    {
        if (validParams?.Length > 0)
            return $"The only active sorting options are [{string.Join(", ", validParams)}] and no repeate field.";
        return "There are no relevant columns to Sort on. Please remove value of the SortBy parameter.";
    }

    protected abstract void Validations();
}
