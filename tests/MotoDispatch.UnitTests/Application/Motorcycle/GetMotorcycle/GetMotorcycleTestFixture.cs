using MotoDispatch.UnitTests.Application.Motorcycle.Common;

namespace MotoDispatch.UnitTests.Application.GetMotorcycle;

[CollectionDefinition(nameof(GetMotorcycleTestFixture))]
public class GetMotorcycleTestFixtureCollection :
    ICollectionFixture<GetMotorcycleTestFixture>
{
}

public class GetMotorcycleTestFixture
    : MotorcycleUseCasesBaseFixture
{
}