using MotoDispatch.UnitTests.Common;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Domain.Entity.RentalPlan.CreateRentalPlan;

[CollectionDefinition(nameof(CreateRentalPlanTestFixture))]
public class RentalPlanTestFixtureCollection
    : ICollectionFixture<CreateRentalPlanTestFixture>
{
}

public class CreateRentalPlanTestFixture : BaseFixture
{
    public CreateRentalPlanTestFixture() : base() { }

    public DomainEntity.RentalPlan CreateValidRentalPlan()
    {
        return new DomainEntity.RentalPlan(
            days: 7,
            dailyRate: 30m,
            penaltyRate: 0.2m,
            fixedAdditionalRate: 50m
        );
    }
}