using MediatR;
using MotoDispatch.Application.UseCases.Rental.CreateRental.Common;

namespace MotoDispatch.Application.UseCases.Rental.CreateRental;

public class CreateRentalInput : IRequest<RentalModelOutput>
{
    public CreateRentalInput(Guid deliveryDriverId, Guid motorcycleId, int days)
    {
        DeliveryDriverId = deliveryDriverId;
        MotorcycleId = motorcycleId;
        Days = days;
    }

    public Guid DeliveryDriverId { get; set; }
    public Guid MotorcycleId { get; set; }
    public int Days { get; set; }
}