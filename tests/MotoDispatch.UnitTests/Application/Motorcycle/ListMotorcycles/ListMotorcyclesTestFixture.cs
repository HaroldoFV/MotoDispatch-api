using MotoDispatch.Application.UseCases.Motorcycle.ListMotorcycles;
using MotoDispatch.Domain.SeedWork.SearchableRepository;
using MotoDispatch.UnitTests.Application.Motorcycle.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Application.Motorcycle.ListMotorcycles;

[CollectionDefinition(nameof(ListMotorcyclesTestFixture))]
public class ListMotorcyclesTestFixtureCollection
    : ICollectionFixture<ListMotorcyclesTestFixture>
{
}

public class ListMotorcyclesTestFixture : MotorcycleUseCasesBaseFixture
{
    public List<DomainEntity.Motorcycle> GetExampleMotorcyclesList(int length = 10)
    {
        var list = new List<DomainEntity.Motorcycle>();

        for (int i = 0; i < length; i++)
            list.Add(GetExampleMotorcycle());
        return list;
    }

    public ListMotorcyclesInput GetExampleInput()
    {
        var random = new Random();

        return new ListMotorcyclesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}