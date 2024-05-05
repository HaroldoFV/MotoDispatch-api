using MotoDispatch.UnitTests.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Domain.Entity.Rental.CreateRental;

[CollectionDefinition(nameof(CreateRentalTestFixture))]
public class RentalTestFixtureCollection
    : ICollectionFixture<CreateRentalTestFixture>
{
}

public class CreateRentalTestFixture : BaseFixture
{
    public CreateRentalTestFixture() : base() { }

    public DomainEntity.RentalPlan CreateValidRentalPlan()
    {
        return new DomainEntity.RentalPlan(7, 30.00m, 0.20m, 50.00m);
    }

    public DomainEntity.Rental CreateValidRental()
    {
        return new DomainEntity.Rental(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            CreateValidRentalPlan()
        );
    }
}