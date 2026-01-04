namespace Architecture.Application.Abstractions.Query;

public sealed record class TSqlQueryBuilder
{
    private readonly StringBuilder _sqlQueryBuilder;
    private bool _hasWhereCondition = false;

    private TSqlQueryBuilder()
        => _sqlQueryBuilder = new StringBuilder();

    public static TSqlQueryBuilder CreateQuery()
        => new TSqlQueryBuilder();

    public TSqlQueryBuilder Initialize(string query)
    {
        _sqlQueryBuilder.AppendLine(query);
        return this;
    }

    public TSqlQueryBuilder Where(string whereCondition)
    {
        if (_hasWhereCondition)
            throw new InvalidOperationException($"Can't use {nameof(Where)} more than by 1.");

        if (!string.IsNullOrWhiteSpace(whereCondition))
            return AddWhereCondition(whereCondition);
        return this;
    }

    public TSqlQueryBuilder Where(bool ifCondition, string whereCondition)
    {
        if (ifCondition)
            return Where(whereCondition);
        return this;
    }

    public TSqlQueryBuilder AND_Where(string propName, string condition)
    {
        if (IsValidPropCondition(propName, condition))
            return AddWhereCondition
                (
                    _hasWhereCondition ? AND_Prefix(propName) : propName,
                    condition
                );
        return this;
    }

    public TSqlQueryBuilder AND_Where(bool ifCondition, string propName, string thenCondition)
    {
        if (ifCondition)
            return AND_Where(propName, thenCondition);
        return this;
    }

    public TSqlQueryBuilder OR_Where(string propName, string condition)
    {
        if (IsValidPropCondition(propName, condition))
            return AddWhereCondition
                (
                    _hasWhereCondition ? OR_Prefix(propName) : propName,
                    condition
                );
        return this;
    }

    public TSqlQueryBuilder OR_Where(bool ifCondition, string propName, string thenCondition)
    {
        if (ifCondition)
            return OR_Where(propName, thenCondition);
        return this;
    }

    public TSqlQueryBuilder AND_Where(string whereCondition)
    {
        if (!string.IsNullOrWhiteSpace(whereCondition))
            return AddWhereCondition
                (
                    _hasWhereCondition
                        ? AND_Prefix(whereCondition)
                        : whereCondition
                );
        return this;
    }

    public TSqlQueryBuilder AND_Where(bool ifCondition, string whereCondition)
    {
        if (ifCondition)
            return AND_Where(whereCondition);
        return this;
    }

    public TSqlQueryBuilder OR_Where(string whereCondition)
    {
        if (!string.IsNullOrWhiteSpace(whereCondition))
            return AddWhereCondition
                (
                    _hasWhereCondition
                        ? OR_Prefix(whereCondition)
                        : whereCondition
                );
        return this;
    }

    public TSqlQueryBuilder OR_Where(bool ifCondition, string whereCondition)
    {
        if (ifCondition)
            return OR_Where(whereCondition);
        return this;
    }

    public TSqlQueryBuilder AppendQuery(string customQuery)
    {
        if (!string.IsNullOrWhiteSpace(customQuery))
            _sqlQueryBuilder.AppendLine(customQuery);
        return this;
    }

    public TSqlQueryBuilder AppendQuery(bool ifCondition, string customQuery)
    {
        if (ifCondition)
            return AppendQuery(customQuery);
        return this;
    }

    public TSqlQueryBuilder IgnoreSoftDeletedItems(string aliasName)
    {
        if (string.IsNullOrWhiteSpace(aliasName))
            return AND_Where(aliasName, DbUtil.IgnoreSoftDeleted(aliasName));
        return this;
    }

    public TSqlQueryBuilder AppendPagination<TResponse>(ICollectionQueryRequest<TResponse> request, ISortable? defaultSort)
        where TResponse : notnull
    {
        ISortable? validSort = request;

        if (validSort?.SortBy?.Length == 0)
        {
            if (defaultSort?.SortBy?.Length == 0)
                throw new ArgumentNullException(nameof(defaultSort));

            if (defaultSort!.SortBy!.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Can't use Empty or Null value in default sort items.");

            if (defaultSort!.SortBy!.Distinct().Count() != defaultSort!.SortBy!.Length)
                throw new ArgumentException("Founded duplicate items in the default sort items.");

            validSort = defaultSort;
        }

        string orderMode = validSort?.SortDesc == true ? "DESC" : "ASC";

        _sqlQueryBuilder.AppendLine($"ORDER BY {string.Join(", ", validSort!.SortBy!.Select(x => $"[{x}]"))} {orderMode}");
        _sqlQueryBuilder.AppendLine($"OFFSET {(request.PageIndex - 1) * request.PageSize} ROWS");
        _sqlQueryBuilder.AppendLine($"FETCH NEXT {request.PageSize + 1} ROWS ONLY");

        return this;
    }

    private TSqlQueryBuilder AddWhereCondition(string customWhereCondition)
    {
        AddOrIgnoreWhereKeyword();

        _sqlQueryBuilder.AppendLine(customWhereCondition);
        return this;
    }

    private TSqlQueryBuilder AddWhereCondition(string propName, string condition)
    {
        AddOrIgnoreWhereKeyword();

        _sqlQueryBuilder.AppendLine($"{propName} {condition}");
        return this;
    }

    private void AddOrIgnoreWhereKeyword()
    {
        if (!_hasWhereCondition)
        {
            _sqlQueryBuilder.Append("WHERE ");
            _hasWhereCondition = true;
        }
    }

    private static string AND_Prefix(string condition)
        => $"{DbUtil.And()}{condition} ";

    private static string OR_Prefix(string condition)
        => $"{DbUtil.Or()}{condition} ";

    private static bool IsValidPropCondition(string propName, string condition)
        => !string.IsNullOrWhiteSpace(propName) && !string.IsNullOrWhiteSpace(condition);

    public override string ToString() => _sqlQueryBuilder.ToString();
}

