using FluentAssertions;
using MotoDispatch.Domain.Exception;
using MotoDispatch.Domain.Validation;

namespace MotoDispatch.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        string fieldName = "Value";
        var value = "Test";

        Action action =
            () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;

        Action action =
            () => DomainValidation.NotNull(value, "FieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null.");
    }

    [Fact(DisplayName = "ValidateStringLengthThrowsIfTooShort")]
    [Trait("Domain", "DomainValidation - Validation")]
    public void ValidateStringLengthThrowsIfTooShort()
    {
        string target = "A";
        Action action = () => DomainValidation.ValidateStringLength(target, 2, 50, "Model");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Model should be between 2 and 50 characters.");
    }

    [Fact(DisplayName = "ValidateStringLengthThrowsIfTooLong")]
    [Trait("Domain", "DomainValidation - Validation")]
    public void ValidateStringLengthThrowsIfTooLong()
    {
        string target = new string('A', 51);
        Action action = () => DomainValidation.ValidateStringLength(target, 2, 50, "Model");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Model should be between 2 and 50 characters.");
    }

    [Fact(DisplayName = "ValidateStringLengthOk")]
    [Trait("Domain", "DomainValidation - Validation")]
    public void ValidateStringLengthOk()
    {
        string target = "Valid Length";
        Action action = () => DomainValidation.ValidateStringLength(target, 2, 50, "Model");
        action.Should().NotThrow();
    }
}