using DomainEntity = MotoDispatch.Domain.Entity;

namespace MotoDispatch.Application.UseCases.Rental.CreateRental.Common;

public class RentalModelOutput(
    Guid id,
    Guid motorcycleId,
    Guid deliveryDriverId,
    Guid rentalPlanId,
    DateTime startDate,
    DateTime endDate,
    DateTime? actualEndDate,
    DateTime createdAt)
{
    public Guid Id { get; set; } = id;
    public Guid MotorcycleId { get; set; } = motorcycleId;
    public Guid DeliveryDriverId { get; set; } = deliveryDriverId;
    public Guid RentalPlanId { get; set; } = rentalPlanId;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public DateTime? ActualEndDate { get; set; } = actualEndDate;
    public DateTime CreatedAt { get; set; } = createdAt;

    public static RentalModelOutput FromRental(DomainEntity.Rental createRental)
    {
        return new(
            createRental.Id,
            createRental.MotorcycleId,
            createRental.DeliveryDriverId,
            createRental.RentalPlanId,
            createRental.StartDate,
            createRental.EndDate,
            createRental.ActualEndDate,
            createRental.CreatedAt
        );
    }
}