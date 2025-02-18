namespace Application.Common.Models.Errors;

public sealed record ValidationError(string PropertyName, IEnumerable<string> ErrorMessages)
{
    public ValidationError(string propertyName, string errorMessage)
        : this(propertyName, new List<string> { errorMessage })
    {
    }
}