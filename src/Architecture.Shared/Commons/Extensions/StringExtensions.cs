namespace Architecture.Shared.Commons.Extensions;

public static class StringExtensions
{
    // Converts "CamelCase" to "camel_case"
    public static string ToUnderscoreCase(this string input)
        => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLowerInvariant();
}
