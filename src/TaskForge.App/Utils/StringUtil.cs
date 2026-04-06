/*
    String Utilities class provides reusable helper methods for common
    string operations such as validation, formatting, and normalization.
*/

using System.Text;
using System.Text.RegularExpressions;

namespace TaskForge.App.Utils;

public static class StringUtil
{
    /// <summary>
    /// Checks if a string is null, empty, or whitespace.
    /// </summary>
    public static bool IsNullOrWhiteSpace(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Safely trims a string (returns empty string if null).
    /// </summary>
    public static string SafeTrim(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Ensures a string is not null. Returns default value if it is.
    /// </summary>
    public static string DefaultIfNull(string? value, string defaultValue = "")
    {
        return value ?? defaultValue;
    }

    /// <summary>
    /// Compares two strings ignoring case.
    /// </summary>
    public static bool EqualsIgnoreCase(string? a, string? b)
    {
        return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if a string contains another string (case-insensitive).
    /// </summary>
    public static bool ContainsIgnoreCase(string source, string value)
    {
        if (IsNullOrWhiteSpace(source) || IsNullOrWhiteSpace(value))
            return false;

        return source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Capitalizes the first letter of a string.
    /// </summary>
    public static string Capitalize(string value)
    {
        if (IsNullOrWhiteSpace(value))
            return string.Empty;

        value = value.Trim();

        return char.ToUpper(value[0]) + value.Substring(1);
    }

    /// <summary>
    /// Joins strings with a separator, ignoring null or empty values.
    /// </summary>
    public static string JoinNonEmpty(string separator, params string?[] values)
    {
        return string.Join(separator, values.Where(v => !IsNullOrWhiteSpace(v)));
    }

    /// <summary>
    /// Generates a random string of given length.
    /// </summary>
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        StringBuilder result = new();

        Random random = new();

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Removes all whitespace from a string.
    /// </summary>
    public static string RemoveWhitespace(string value)
    {
        if (IsNullOrWhiteSpace(value))
            return string.Empty;

        return Regex.Replace(value, @"\s+", "");
    }

    /// <summary>
    /// Shortens a string to a max length with optional ellipsis.
    /// </summary>
    public static string Truncate(string value, int maxLength, bool addEllipsis = true)
    {
        if (IsNullOrWhiteSpace(value))
            return string.Empty;

        if (value.Length <= maxLength)
            return value;

        return addEllipsis
            ? value.Substring(0, maxLength) + "..."
            : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Normalizes a string for safe comparisons (trim + lowercase).
    /// </summary>
    public static string Normalize(string? value)
    {
        return SafeTrim(value).ToLowerInvariant();
    }
}