using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed record PhoneNumber
{
    private const string Pattern = @"^\+?[1-9]\d{1,14}$";

    public string Value { get; init; }

    public PhoneNumber(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid phone number", nameof(value));
        }

        Value = value;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Regex.IsMatch(value, Pattern);
    }

    public override string ToString() => Value;

    public static PhoneNumber Create(string value) => new(value);
}