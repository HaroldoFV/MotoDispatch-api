using MotoDispatch.IntegrationTests.UseCases.Motorcycle.Common;

namespace MotoDispatch.IntegrationTests.UseCases.Motorcycle.GetMotorcycle;

[CollectionDefinition(nameof(GetMotorcycleTestFixture))]
public class GetCMotorcycleTestFixtureCollection
    : ICollectionFixture<GetMotorcycleTestFixture>
{
}
public class GetMotorcycleTestFixture
: MotorcycleUseCasesBaseFixture
{

}