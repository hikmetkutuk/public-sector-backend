using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed record FullName
{
    private const string Pattern = @"^[A-Z][a-z]+\s[A-Z][a-z]+$";
    private const int MinLength = 2;
    private const int MaxLength = 100;

    public string FirstName { get; init; }
    public IReadOnlyList<string> MiddleNames { get; init; } = Array.Empty<string>();
    public string LastName { get; init; }

    private FullName(string firstName, IReadOnlyList<string> middleNames, string lastName)
    {
        if (!IsValid(firstName))
            throw new ArgumentException("Invalid first name", nameof(firstName));
        if (!IsValid(lastName))
            throw new ArgumentException("Invalid last name", nameof(lastName));

        FirstName = firstName;
        MiddleNames = middleNames ?? Array.Empty<string>();
        LastName = lastName;
    }

    private static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (value.Length < MinLength || value.Length > MaxLength)
            return false;

        return Regex.IsMatch(value, Pattern);
    }

    public override string ToString()
    {
        var middleNames = string.Join(" ", MiddleNames);
        return string.IsNullOrWhiteSpace(middleNames)
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {middleNames} {LastName}";
    }
}