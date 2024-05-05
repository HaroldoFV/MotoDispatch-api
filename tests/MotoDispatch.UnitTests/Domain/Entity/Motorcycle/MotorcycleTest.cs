using FluentAssertions;
using MotoDispatch.Domain.Exception;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Domain.Entity.Motorcycle;

[Collection(nameof(MotorcycleTestFixture))]
public class MotorcycleTest(MotorcycleTestFixture motorcycleTestFixture)
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    public void Instantiate()
    {
        var validMotorcycle = motorcycleTestFixture.GetValidMotorcycle();
        var datetimeBefore = DateTime.Now;

        var motorcycle =
            new DomainEntity.Motorcycle(validMotorcycle.Year, validMotorcycle.LicensePlate, validMotorcycle.Model);
        var datetimeAfter = DateTime.Now.ToUniversalTime().AddSeconds(1);

        motorcycle.Should().NotBeNull();
        motorcycle.Year.Should().Be(validMotorcycle.Year);
        motorcycle.LicensePlate.Should().Be(validMotorcycle.LicensePlate);
        motorcycle.Id.Should().NotBeEmpty();
        motorcycle.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (motorcycle.CreatedAt >= datetimeBefore).Should().BeTrue();
        (motorcycle.CreatedAt <= datetimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenModelIsEmpty))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateErrorWhenModelIsEmpty(string? model)
    {
        Action action =
            () => new DomainEntity.Motorcycle(2013, "AB1C234", model!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Model should not be empty or null.");
    }

    [Theory(DisplayName = nameof(InstantiateErrorForInvalidYear))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    [InlineData(1899, "Year should be greater than 1900.")]
    [InlineData(2050, "Year should be less than or equal to current year.")]
    public void InstantiateErrorForInvalidYear(int year, string expectedMessage)
    {
        Action action =
            () => new DomainEntity.Motorcycle(year, "ABC1D23", "Harley-Davidson Road King");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

    [Fact(DisplayName = nameof(InstantiateErrorForInvalidModelLength))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    public void InstantiateErrorForInvalidModelLength()
    {
        var createMotorcycle =
            motorcycleTestFixture.CreateInvalidModelMotorcycle;

        createMotorcycle.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Model should be between 2 and 50 characters.");
    }

    [Theory(DisplayName = nameof(InstantiateErrorForInvalidLicensePlateInput))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    [MemberData(nameof(InvalidLicensePlateData))]
    public void InstantiateErrorForInvalidLicensePlateInput(string? licensePlate, string model,
        string expectedMessage)
    {
        Action action =
            () => new DomainEntity.Motorcycle(2013, licensePlate!, model);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

    public static IEnumerable<object?[]> InvalidLicensePlateData()
    {
        yield return new object?[] {"", "BMW S1000RR", "LicensePlate should not be empty or null"};
        yield return new object?[] {null, "BMW S1000RR", "LicensePlate should not be empty or null"};
        yield return new object[] {"   ", "BMW S1000RR", "LicensePlate should not be empty or null"};
        yield return new object[] {"YZF1000", "Yamaha R1", "LicensePlate must be in the format AAA-9999 or ABC1D23."};
        yield return new object[] {"123-ABCD", "Yamaha R6", "LicensePlate must be in the format AAA-9999 or ABC1D23."};

        yield return new object[]
        {
            "ABCDE1234", "Kawasaki Ninja", "LicensePlate must be in the format AAA-9999 or ABC1D23."
        };
    }

    [Fact(DisplayName = nameof(UpdateLicensePlate))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    public void UpdateLicensePlate()
    {
        var motorcycle = motorcycleTestFixture.GetValidMotorcycle();
        var newMotorcycle = motorcycleTestFixture.GetValidMotorcycle();

        motorcycle.UpdateLicensePlate(newMotorcycle.LicensePlate);
        newMotorcycle.LicensePlate.Should().Be(motorcycle.LicensePlate);
    }

    [Theory(DisplayName = nameof(UpdateErrorForInvalidLicensePlateInput))]
    [Trait("Domain", "Motorcycle - Aggregates")]
    [MemberData(nameof(InvalidLicensePlateUpdateData))]
    public void UpdateErrorForInvalidLicensePlateInput(string? licensePlate, string expectedMessage)
    {
        var motorcycle = motorcycleTestFixture.GetValidMotorcycle();

        Action action =
            () => motorcycle.UpdateLicensePlate(licensePlate);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

    public static IEnumerable<object?[]> InvalidLicensePlateUpdateData()
    {
        yield return ["", "LicensePlate should not be empty or null"];
        yield return [null, "LicensePlate should not be empty or null"];
        yield return ["  ", "LicensePlate should not be empty or null"];
        yield return ["YZF1000", "LicensePlate must be in the format AAA-9999 or ABC1D23."];
        yield return ["123-ABCD", "LicensePlate must be in the format AAA-9999 or ABC1D23."];
        yield return ["ABCDE1234", "LicensePlate must be in the format AAA-9999 or ABC1D23."];
    }
}