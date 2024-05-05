using MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;

namespace MotoDispatch.IntegrationTests.UseCases.Motorcycle.CreateMotorcycle;

public class CreateMotorcycleTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        yield return new object[]
        {
            new CreateMotorcycleInput(1800, "ABC-1234", "Valid Model"), "Year should be greater than 1900"
        };

        yield return new object[]
        {
            new CreateMotorcycleInput(2025, "ABC-1234", "Valid Model"),
            "Year should be less than or equal to current year"
        };

        yield return new object[]
        {
            new CreateMotorcycleInput(2020, "123456", "Valid Model"),
            "LicensePlate must be in the format AAA-9999 or ABC1D23."
        };

        yield return new object[]
        {
            new CreateMotorcycleInput(2020, "ABC-1234", "A"), "Model must be between 2 and 50 characters."
        };
    }
}