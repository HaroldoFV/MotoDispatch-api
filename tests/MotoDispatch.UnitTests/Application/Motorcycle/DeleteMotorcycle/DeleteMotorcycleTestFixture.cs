using MotoDispatch.UnitTests.Application.Motorcycle.Common;

namespace MotoDispatch.UnitTests.Application.Motorcycle.DeleteMotorcycle;

[CollectionDefinition(nameof(DeleteMotorcycleTestFixture))]
public class DeleteMotorcycleTestFixtureCollection
    : ICollectionFixture<DeleteMotorcycleTestFixture>
{
}

public class DeleteMotorcycleTestFixture
    : MotorcycleUseCasesBaseFixture
{
}