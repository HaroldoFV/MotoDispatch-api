using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.CreateDeliveryDriver;

public interface ICreateDeliveryDriver
    : IRequestHandler<CreateDeliveryDriverInput, DeliveryDriverModelOutput>
{
}