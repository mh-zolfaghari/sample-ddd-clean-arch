namespace Architecture.Shared.Commons.Extensions;

// This extension method converts a given string into a JSON-compatible key format.
public static class JsonKeyExtensions
{
    private static readonly Regex InvalidCharsRegex = new(@"[^a-zA-Z0-9_]", RegexOptions.Compiled);
    private static readonly Regex MultiUnderscoreRegex = new(@"_+", RegexOptions.Compiled);

    // Converts the input string to a JSON key format.
    public static string ToJsonKey(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "unknown";

        input = input.Trim();
        input = input
            .Replace(" ", "_")
            .Replace("-", "_")
            .Replace(".", "_")
            .Replace("/", "_");
        input = InvalidCharsRegex.Replace(input, string.Empty);
        input = input.ToUnderscoreCase();
        input = MultiUnderscoreRegex.Replace(input, "_");
        input = input.Trim('_');

        return string.IsNullOrEmpty(input) ? "unknown" : input;
    }
}
