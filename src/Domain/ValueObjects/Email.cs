using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed record Email
{
    private const string Pattern =
        @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

    public string Value { get; init; }

    public Email(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid email address", nameof(value));
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return Regex.IsMatch(value, Pattern, RegexOptions.IgnoreCase);
    }

    public override string ToString() => Value;

    public static Email Create(string value)
    {
        return new Email(value);
    }
}