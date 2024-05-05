using MotoDispatch.Application.UseCases.Motorcycle.UpdateMotorcycle;
using MotoDispatch.UnitTests.Application.Motorcycle.Common;

namespace MotoDispatch.UnitTests.Application.Motorcycle.UpdateMotorcycle;

[CollectionDefinition(nameof(UpdateMotorcycleTestFixture))]
public class UpdateMotorcycleTestFixtureCollection
    : ICollectionFixture<UpdateMotorcycleTestFixture>
{
}

public class UpdateMotorcycleTestFixture
    : MotorcycleUseCasesBaseFixture
{
    public UpdateMotorcycleInput GetValidInput(Guid? id = null)
        => new(
            id ?? Guid.NewGuid(),
            GetValidLicensePlate(),
            GetValidModel(),
            GetValidYear());
}