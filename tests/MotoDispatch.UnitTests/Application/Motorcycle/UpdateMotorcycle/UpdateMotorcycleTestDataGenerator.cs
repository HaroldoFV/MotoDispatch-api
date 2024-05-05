using MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;

namespace MotoDispatch.UnitTests.Application.Motorcycle.UpdateMotorcycle;

public class UpdateMotorcycleTestDataGenerator
{
    public static IEnumerable<object[]> GetMotorcycleToUpdate(int times = 10)
    {
        var fixture = new UpdateMotorcycleTestFixture();

        for (int indice = 0; indice < times; indice++)
        {
            var exampleMotorcycle = fixture.GetExampleMotorcycle();
            var exampleInput = fixture.GetValidInput(exampleMotorcycle.Id);
            yield return new object[] {exampleMotorcycle, exampleInput};
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        yield return new object[]
        {
            new UpdateMotorcycleInput(Guid.NewGuid(), "ABC-1234", "Valid Model", 1800),
            "Year should be greater than 1900."
        };

        yield return new object[]
        {
            new UpdateMotorcycleInput(Guid.NewGuid(), "ABC-1234", "Valid Model", 2025),
            "Year should be less than or equal to current year."
        };

        yield return new object[]
        {
            new UpdateMotorcycleInput(Guid.NewGuid(), "123456", "Valid Model", 2020),
            "LicensePlate must be in the format AAA-9999 or ABC1D23."
        };

        yield return new object[]
        {
            new UpdateMotorcycleInput(Guid.NewGuid(), "ABC-1234", "A", 2020),
            "Model should be between 2 and 50 characters."
        };
    }
}