using FluentAssertions;
using MotoDispatch.Application.UseCases.Motorcycle.GetMotorcycle;
using MotoDispatch.UnitTests.Application.GetMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.GetMotorcycle;

[Collection(nameof(GetMotorcycleTestFixture))]
public class GetMotorcycleInputValidatorTest
{
    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetMotorcycleValidation - UseCases")]
    public void ValidationOk()
    {
        var validInput = new GetMotorcycleInput(Guid.NewGuid());
        var validator = new GetMotorcycleInputValidator();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetMotorcycleValidation - UseCases")]
    public void InvalidWhenEmptyGuidId()
    {
        var invalidInput = new GetMotorcycleInput(Guid.Empty);
        var validator = new GetMotorcycleInputValidator();

        var validationResult = validator.Validate(invalidInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);

        validationResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");
    }
}