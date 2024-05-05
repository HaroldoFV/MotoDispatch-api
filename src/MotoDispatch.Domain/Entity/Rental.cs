using MotoDispatch.Domain.Exception;
using MotoDispatch.Domain.SeedWork;
using MotoDispatch.Domain.Validation;

namespace MotoDispatch.Domain.Entity;

public class Rental : AggregateRoot
{
    public Guid MotorcycleId { get; private set; }
    public Guid DeliveryDriverId { get; private set; }
    public Guid RentalPlanId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime? ActualEndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public RentalPlan RentalPlan { get; private set; }

    public Rental(Guid motorcycleId, Guid deliveryDriverId, Guid rentalPlanId, RentalPlan rentalPlan)
    {
        MotorcycleId = motorcycleId;
        DeliveryDriverId = deliveryDriverId;
        RentalPlanId = rentalPlanId;
        RentalPlan = rentalPlan;

        Validate();

        StartDate = DateTime.UtcNow.AddDays(1).Date;
        EndDate = StartDate.AddDays(RentalPlan.Days).AddSeconds(-1);
        CreatedAt = DateTime.Now.ToUniversalTime();
    }

    private Rental() { }

    public void CompleteRental(DateTime returnDate)
    {
        if (returnDate > StartDate.AddYears(1))
            throw new EntityValidationException("ReturnDate is invalid because it is too far in the future.");

        if (returnDate < StartDate)
            throw new EntityValidationException("ReturnDate should not be before StartDate.");

        ActualEndDate = returnDate;
    }

    private void Validate()
    {
        DomainValidation.NotEmptyGuid(MotorcycleId, nameof(MotorcycleId));
        DomainValidation.NotEmptyGuid(DeliveryDriverId, nameof(DeliveryDriverId));
        DomainValidation.NotEmptyGuid(RentalPlanId, nameof(RentalPlanId));
        DomainValidation.NotNull(RentalPlan, nameof(RentalPlan));

        if (RentalPlan.Days <= 0 || RentalPlan.DailyRate <= 0)
            throw new EntityValidationException("Invalid rental plan provided.");
    }
}