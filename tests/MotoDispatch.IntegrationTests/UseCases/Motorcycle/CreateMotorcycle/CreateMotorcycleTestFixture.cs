using MotoDispatch.Application.UseCases.Motorcycle.CreateMotorcycle;
using MotoDispatch.IntegrationTests.UseCases.Motorcycle.Common;

namespace MotoDispatch.IntegrationTests.UseCases.Motorcycle.CreateMotorcycle;

[CollectionDefinition(nameof(CreateMotorcycleTestFixture))]
public class CreateMotorcycleTestFixtureCollection
    : ICollectionFixture<CreateMotorcycleTestFixture>
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