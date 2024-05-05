using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.UpdateDeliveryDriver;

public interface IUpdateDeliveryDriver
    : IRequestHandler<UpdateDeliveryDriverInput, DeliveryDriverModelOutput>
{
}