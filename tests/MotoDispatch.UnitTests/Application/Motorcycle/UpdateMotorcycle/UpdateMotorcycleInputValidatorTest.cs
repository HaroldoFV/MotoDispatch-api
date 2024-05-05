using FluentAssertions;
using FluentValidation;
using MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.UpdateMotorcycle;

[Collection(nameof(UpdateMotorcycleTestFixture))]
public class UpdateMotorcycleInputValidatorTest(UpdateMotorcycleTestFixture fixture)
{
    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateMotorcycleInputValidator - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var input = fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateMotorcycleInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);

        validateResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");
    }

    [Fact(DisplayName = nameof(ValidateWhenValid))]
    [Trait("Application", "UpdateMotorcycleInputValidator - Use Cases")]
    public void ValidateWhenValid()
    {
        var input = fixture.GetValidInput();
        var validator = new UpdateMotorcycleInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}