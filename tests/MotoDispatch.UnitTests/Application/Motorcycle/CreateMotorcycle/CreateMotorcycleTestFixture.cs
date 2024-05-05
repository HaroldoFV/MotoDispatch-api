using MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;
using MotoDispatch.UnitTests.Application.Motorcycle.Common;

namespace MotoDispatch.UnitTests.Application.Motorcycle.CreateMotorcycle;

[CollectionDefinition(nameof(CreateMotorcycleTestFixture))]
public class CreateMotorcycleTestFixtureCollection :
    ICollectionFixture<CreateMotorcycleTestFixture>
{
}

public class CreateMotorcycleTestFixture
    : MotorcycleUseCasesBaseFixture
{
    public CreateMotorcycleInput GetInput()
    {
        var year = GetValidYear();
        var licensePlate = GetValidLicensePlate();
        var model = GetValidModel();

        return new(year, licensePlate, model);
    }
}