using System.Text.RegularExpressions;
using MotoDispatch.Domain.Exception;

namespace MotoDispatch.Domain.Validation;

public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
            throw new EntityValidationException(
                $"{fieldName} should not be null.");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
            throw new EntityValidationException(
                $"{fieldName} should not be empty or null.");
    }

    public static void NotEmptyGuid(Guid target, string fieldName)
    {
        if (target == Guid.Empty)
            throw new EntityValidationException($"{fieldName} should not be empty.");
    }

    

    public static void MinLength(string target, int minLength, string fieldName)
    {
        if (target.Length < minLength)
            throw new EntityValidationException($"{fieldName} should be at least {minLength} characters long.");
    }

    public static void MaxLength(string target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength)
            throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characters long.");
    }

    public static void ValidateStringLength(string target, int min, int max, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{fieldName} should not be empty or null.");

        if (target.Length < min || target.Length > max)
        {
            throw new EntityValidationException($"{fieldName} should be between {min} and {max} characters.");
        }
    }

    public static void GreaterThanZero(decimal value, string fieldName)
    {
        if (value <= 0)
            throw new EntityValidationException($"{fieldName} should be greater than zero.");
    }
}