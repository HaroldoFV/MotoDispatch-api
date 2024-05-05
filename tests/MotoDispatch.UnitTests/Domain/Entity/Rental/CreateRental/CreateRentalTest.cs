using FluentAssertions;
using MotoDispatch.Domain.Exception;
using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.UnitTests.Domain.Entity.Rental.CreateRental;

[Collection(nameof(CreateRentalTestFixture))]
public class CreateRentalTest(CreateRentalTestFixture fixture)
{
    [Fact(DisplayName = "Instantiate")]
    [Trait("Domain", "Rental - Aggregates")]
    public void Instantiate()
    {
        var rental = fixture.CreateValidRental();

        rental.Should().NotBeNull();
        rental.MotorcycleId.Should().NotBeEmpty();
        rental.DeliveryDriverId.Should().NotBeEmpty();
        rental.RentalPlanId.Should().NotBeEmpty();
        rental.StartDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(1).Date, TimeSpan.FromSeconds(1));

        rental.EndDate.Should()
            .BeCloseTo(
                rental.StartDate
                    .AddDays(7)
                    .AddSeconds(-1),
                TimeSpan.FromSeconds(1)
            );
    }

    [Fact(DisplayName = nameof(InstantiateThrowsIfMotorcycleIdIsEmpty))]
    [Trait("Domain", "Rental - Aggregates")]
    public void InstantiateThrowsIfMotorcycleIdIsEmpty()
    {
        var validGuid = Guid.NewGuid();
        var rentalPlan = fixture.CreateValidRentalPlan();

        Action act = () => new DomainEntity.Rental(
            motorcycleId: Guid.Empty,
            deliveryDriverId: validGuid,
            rentalPlanId: validGuid,
            rentalPlan: rentalPlan
        );

        act.Should()
            .Throw<EntityValidationException>()
            .WithMessage("MotorcycleId should not be empty.");
    }

    [Fact(DisplayName = nameof(InstantiateThrowsIfDeliveryDriverIdIsEmpty))]
    [Trait("Domain", "Rental - Aggregates")]
    public void InstantiateThrowsIfDeliveryDriverIdIsEmpty()
    {
        var validGuid = Guid.NewGuid();
        var rentalPlan = fixture.CreateValidRentalPlan();

        Action act = () => new DomainEntity.Rental(
            motorcycleId: validGuid,
            deliveryDriverId: Guid.Empty,
            rentalPlanId: validGuid,
            rentalPlan: rentalPlan
        );

        act.Should()
            .Throw<EntityValidationException>()
            .WithMessage("DeliveryDriverId should not be empty.");
    }

    [Fact(DisplayName = nameof(InstantiateThrowsIfRentalPlanIdIsEmpty))]
    [Trait("Domain", "Rental - Aggregates")]
    public void InstantiateThrowsIfRentalPlanIdIsEmpty()
    {
        var validGuid = Guid.NewGuid();
        var rentalPlan = fixture.CreateValidRentalPlan();

        Action act = () => new DomainEntity.Rental(
            motorcycleId: validGuid,
            deliveryDriverId: validGuid,
            rentalPlanId: Guid.Empty,
            rentalPlan: rentalPlan
        );

        act.Should()
            .Throw<EntityValidationException>()
            .WithMessage("RentalPlanId should not be empty.");
    }

    [Fact(DisplayName = nameof(EndDateIsCorrectlyCalculated))]
    [Trait("Domain", "Rental - Aggregates")]
    public void EndDateIsCorrectlyCalculated()
    {
        var startDate = DateTime.UtcNow.AddDays(1).Date;
        var rental = fixture.CreateValidRental();

        rental.EndDate.Should().Be(startDate.AddDays(7).AddSeconds(-1));
    }

    [Fact(DisplayName = nameof(CompleteRentalSetsActualEndDate))]
    [Trait("Domain", "Rental - Aggregates")]
    public void CompleteRentalSetsActualEndDate()
    {
        var rental = fixture.CreateValidRental();
        var returnDate = DateTime.UtcNow.AddDays(5);

        rental.CompleteRental(returnDate);

        rental.ActualEndDate.Should().Be(returnDate);
        rental.ActualEndDate.Should().BeAfter(rental.StartDate);
    }

    [Fact(DisplayName = nameof(InstantiateThrowsIfRentalPlanIsNull))]
    [Trait("Domain", "Rental - Aggregates")]
    public void InstantiateThrowsIfRentalPlanIsNull()
    {
        Action act = () => new DomainEntity.Rental(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null);

        act.Should()
            .Throw<EntityValidationException>()
            .WithMessage("RentalPlan should not be null.");
    }

    [Fact(DisplayName = nameof(CompleteRentalThrowsIfActualEndDateIsBeforeStartDate))]
    [Trait("Domain", "Rental - Aggregates")]
    public void CompleteRentalThrowsIfActualEndDateIsBeforeStartDate()
    {
        var rental = fixture.CreateValidRental();
        var returnDate = rental.StartDate.AddDays(-1);

        Action act = () => rental.CompleteRental(returnDate);

        act.Should()
            .Throw<EntityValidationException>()
            .WithMessage("ReturnDate should not be before StartDate.");
    }

    [Fact(DisplayName = "CompleteRentalThrowsIfReturnDateIsInvalid")]
    [Trait("Domain", "Rental - Aggregates")]
    public void CompleteRentalThrowsIfReturnDateIsInvalid()
    {
        var rental = fixture.CreateValidRental();

        var invalidReturnDate =
            DateTime.UtcNow.AddDays(1000);

        Action act = () => rental.CompleteRental(invalidReturnDate);

        act.Should()
            .Throw<EntityValidationException>()
            .WithMessage("ReturnDate is invalid because it is too far in the future.");
    }
}