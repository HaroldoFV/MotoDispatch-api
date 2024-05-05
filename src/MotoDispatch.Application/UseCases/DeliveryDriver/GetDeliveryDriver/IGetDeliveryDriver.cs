using MediatR;
using MotoDispatch.Application.UseCases.DeliveryDriver.Common;

namespace MotoDispatch.Application.UseCases.DeliveryDriver.GetDeliveryDriver;

public interface IGetDeliveryDriver :
    IRequestHandler<GetDeliveryDriverInput, DeliveryDriverModelOutput>
{
}