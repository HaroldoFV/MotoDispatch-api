using FluentAssertions;
using MotoDispatch.Domain.Exception;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Domain.Entity.RentalPlan.CreateRentalPlan;

[Collection(nameof(CreateRentalPlanTestFixture))]
public class CreateRentalPlanTest(CreateRentalPlanTestFixture createRentalPlanTestFixture)
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "RentalPlan - Aggregates")]
    public void Instantiate()
    {
        var validRentalPlan = createRentalPlanTestFixture.CreateValidRentalPlan();

        var rentalPlan = new DomainEntity.RentalPlan(
            validRentalPlan.Days,
            validRentalPlan.DailyRate,
            validRentalPlan.PenaltyRate,
            validRentalPlan.FixedAdditionalRate
        );

        rentalPlan.Should().NotBeNull();
        rentalPlan.Days.Should().Be(validRentalPlan.Days);
        rentalPlan.DailyRate.Should().Be(validRentalPlan.DailyRate);
        rentalPlan.PenaltyRate.Should().Be(validRentalPlan.PenaltyRate);
        rentalPlan.FixedAdditionalRate.Should().Be(validRentalPlan.FixedAdditionalRate);
    }

    [Theory(DisplayName = nameof(InstantiateErrorForInvalidValues))]
    [Trait("Domain", "RentalPlan - Aggregates")]
    [InlineData(-0, 30.00, 0.2, "Days must be greater than zero.")]
    [InlineData(7, -30.00, 0.2, "DailyRate must be greater than zero.")]
    [InlineData(7, 0.00, 0.2, "DailyRate must be greater than zero.")]
    [InlineData(7, 30.00, -0.1, "PenaltyRate must not be negative.")]
    public void InstantiateErrorForInvalidValues(int days, decimal dailyRate, decimal penaltyRate,
        string expectedMessage)
    {
        Action action = () => new DomainEntity.RentalPlan(days, dailyRate, penaltyRate, 50.00m);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

    [Theory(DisplayName = "CalculateTotalCost Returns Expected Cost")]
    [Trait("Domain", "RentalPlan - Aggregates")]
    [MemberData(nameof(RentalPlanData))]
    public void CalculateTotalCost_ReturnsExpectedCost(
        int days, decimal dailyRate, decimal penaltyRate, decimal fixedAdditionalRate, int rentedDays, int plannedDays,
        decimal expectedCost)
    {
        var plan = new DomainEntity.RentalPlan(days, dailyRate, penaltyRate, fixedAdditionalRate);

        var totalCost = plan.CalculateTotalCost(rentedDays, plannedDays);

        totalCost.Should().Be(expectedCost);
    }

    public static IEnumerable<object[]> RentalPlanData()
    {
        yield return [7, 30.00m, 0.20m, 50.00m, 7, 7, 210.00m];
        yield return [7, 30.00m, 0.20m, 50.00m, 5, 7, 162.00m];
        yield return [7, 30.00m, 0.20m, 50.00m, 9, 7, 310.00m];
        yield return [15, 28.00m, 0.40m, 50.00m, 15, 15, 420.00m];
        yield return [15, 28.00m, 0.40m, 50.00m, 10, 15, 336.00m];
        yield return [15, 28.00m, 0.40m, 50.00m, 20, 15, 670.00m];
        yield return [30, 22.00m, 0m, 50.00m, 30, 30, 660.00m];
        yield return [30, 22.00m, 0m, 50.00m, 25, 30, 550.00m];
        yield return [30, 22.00m, 0m, 50.00m, 35, 30, 910.00m];
        yield return [45, 20.00m, 0m, 50.00m, 45, 45, 900.00m];
        yield return [45, 20.00m, 0m, 50.00m, 40, 45, 800.00m];
        yield return [45, 20.00m, 0m, 50.00m, 50, 45, 1150.00m];
        yield return [50, 18.00m, 0m, 50.00m, 50, 50, 900.00m];
        yield return [50, 18.00m, 0m, 50.00m, 45, 50, 810.00m];
        yield return [50, 18.00m, 0m, 50.00m, 55, 50, 1150.00m];
    }
}