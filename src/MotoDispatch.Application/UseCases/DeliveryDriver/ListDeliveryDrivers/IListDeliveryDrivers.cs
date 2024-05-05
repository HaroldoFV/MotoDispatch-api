using MediatR;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.ListDeliveryDrivers;

public interface IListDeliveryDrivers
    : IRequestHandler<ListDeliveryDriversInput, ListDeliveryDriversOutput>
{
}