namespace Architecture.Application.Abstractions.Query;

// T-SQL Where Condition helpers
internal static class DbUtil
{
    #region AND
    internal static string And() => $"AND ";
    #endregion

    #region OR
    internal static string Or() => $"OR ";
    #endregion

    #region EQUAL / NOT EQUAL
    internal static string Equal<T>(T value) => $"= {GetSqlAcceptableValue(value)}";
    internal static string Equal<T>(string propName, T value) => $"{ValidatePropName(propName)} {Equal(value)}";
    internal static string NotEqual<T>(T value) => $"<> {GetSqlAcceptableValue(value)}";
    internal static string NotEqual<T>(string propName, T value) => $"{ValidatePropName(propName)} {NotEqual(value)}";
    #endregion

    #region LESS THAN / LESS THAN OR EQUAL
    internal static string LessThan<T>(T value) => $"< {GetSqlAcceptableValue(value)}";
    internal static string LessThan<T>(string propName, T value) => $"{ValidatePropName(propName)} {LessThan(value)}";
    internal static string LessThanOrEqual<T>(T value) => $"<= {GetSqlAcceptableValue(value)}";
    internal static string LessThanOrEqual<T>(string propName, T value) => $"{ValidatePropName(propName)} {LessThanOrEqual(value)}";
    #endregion

    #region GREATER THAN / GREATER THAN OR EQUAL
    internal static string GreaterThan<T>(T value) => $"> {GetSqlAcceptableValue(value)}";
    internal static string GreaterThan<T>(string propName, T value) => $"{ValidatePropName(propName)} {GreaterThan(value)}";
    internal static string GreaterThanOrEqual<T>(T value) => $">= {GetSqlAcceptableValue(value)}";
    internal static string GreaterThanOrEqual<T>(string propName, T value) => $"{ValidatePropName(propName)} {GreaterThanOrEqual(value)}";
    #endregion

    #region BETWEEN
    internal static string Between<T>(T from, T to) where T : notnull => $"BETWEEN {GetSqlAcceptableValue(from)}{And()}{GetSqlAcceptableValue(to)}";
    internal static string Between<T>(string propName, T from, T to) where T : notnull => $"{ValidatePropName(propName)} {Between(from, to)}";
    #endregion

    #region DATE / TIME / DATETIME / DATETIME OFFSET
    internal static string DateRange(string propName, DateOnly? from, DateOnly? to, bool includeEqual = true) => DateTimeRange((propName, from), (propName, to), includeEqual);
    internal static string DateRange((string propName, DateOnly? value) from, (string propName, DateOnly? value) to, bool includeEqual = true) => DateTimeRange(from, to, includeEqual);
    internal static string TimeRange(string propName, TimeOnly from, TimeOnly to, bool includeEqual = true) => DateTimeRange((propName, from), (propName, to), includeEqual);
    internal static string TimeRange((string propName, TimeOnly value) from, (string propName, TimeOnly value) to, bool includeEqual = true) => DateTimeRange(from, to, includeEqual);
    internal static string DateTimeRange(string propName, DateTime from, DateTime to, bool includeEqual = true) => DateTimeRange((propName, from), (propName, to), includeEqual);
    internal static string DateTimeRange((string propName, DateTime value) from, (string propName, DateTime value) to, bool includeEqual = true) => DateTimeRange(from, to, includeEqual);
    internal static string DateTimeOffsetRange(string propName, DateTimeOffset from, DateTimeOffset to, bool includeEqual = true) => DateTimeRange((propName, from), (propName, to), includeEqual);
    internal static string DateTimeOffsetRange((string propName, DateTimeOffset value) from, (string propName, DateTimeOffset value) to, bool includeEqual = true) => DateTimeRange(from, to, includeEqual);

    private static string DateTimeRange<T>((string propName, T value) from, (string propName, T value) to, bool includeEqual = true)
    {
        if (includeEqual)
        {
            if (from.value is null)
                return $"{ValidatePropName(to.propName, "to.propName")} {LessThanOrEqual(to.value)}";
            if (to.value is null)
                return $"{ValidatePropName(from.propName, "from.propName")} {GreaterThanOrEqual(from.value)}";

            return $"{ValidatePropName(from.propName, "from.propName")} {GreaterThanOrEqual(from.value)}{And()}{ValidatePropName(to.propName, "to.propName")} {LessThanOrEqual(to.value)}";
        }

        if (from.value is null)
            return $"{ValidatePropName(to.propName, "to.propName")} {LessThan(to.value)}";
        if (to.value is null)
            return $"{ValidatePropName(from.propName, "from.propName")} {GreaterThan(from.value)}";

        return $"{ValidatePropName(from.propName, "from.propName")} {GreaterThan(from.value)}{And()}{ValidatePropName(to.propName, "to.propName")} {LessThan(to.value)}";
    }
    #endregion

    #region LIKE / NOT LIKE
    internal static string Like(string value) => $"LIKE N'%{value}%'";
    internal static string Like(string propName, string value) => $"{ValidatePropName(propName)} {Like(value)}";
    internal static string NotLike(string value) => $"NOT {Like(value)}";
    internal static string NotLike(string propName, string value) => $"{ValidatePropName(propName)} {NotLike(value)}";
    #endregion

    #region IN / NOT IN
    internal static string In<T>(params T[] values) => $"IN ({string.Join(", ", values.Select(x => $"{GetSqlAcceptableValue(x)}"))})";
    internal static string In<T>(string propName, params T[] values) => $"{ValidatePropName(propName)} {In(values)}";
    internal static string NotIn<T>(params T[] values) => $"NOT {In(values)}";
    internal static string NotIn<T>(string propName, params T[] values) => $"{ValidatePropName(propName)} {NotIn(values)}";
    #endregion

    #region IGNORE SOFT DELETE
    internal static string IgnoreSoftDeleted(string aliasName) => $"{aliasName}.{nameof(IRecordState.RecordState)} <> {(int)RecordState.Deleted} ";
    #endregion

    #region Shared PRIVATE Methods
    static string ValidatePropName(string value, string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(string.IsNullOrWhiteSpace(paramName) ? "propName" : paramName);

        return value;
    }

    private static string GetSqlAcceptableValue<T>(T? value)
    {
        if (value == null)
            return "NULL";

        Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        if (type.IsEnum)
            return $"{Convert.ToInt32(value)}";
        if (type == typeof(bool))
            return $"{(Convert.ToBoolean(value) ? 1 : 0)}";
        if (IsNumericType(type))
            return $"{value}";
        return $"'{value}'";
    }

    private static bool IsNumericType(Type type)
        => type == typeof(byte) ||
           type == typeof(short) ||
           type == typeof(int) ||
           type == typeof(long) ||
           type == typeof(float) ||
           type == typeof(double) ||
           type == typeof(decimal);
    #endregion
}
