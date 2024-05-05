using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;
using MotoDispatch.Application.UseCases.Motorcycle.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.GetDeliveryDriver;

public class GetDeliveryDriverInput(Guid id)
    : IRequest<DeliveryDriverModelOutput>
{
    public Guid Id { get; set; } = id;
}